#!/usr/bin/env python3
"""Resolves and validates release versions against the repository's `v*` tags.

Usage:
  version.py next <bumped>        Prints the next release version (without `v`).
                                  <bumped> is the output of `git cliff --bumped-version`.
                                  When git-cliff has nothing to bump (only skipped commits since the last
                                  tag), it returns an old version; the patch of the latest tag is bumped instead.
  version.py validate <version>   Fails unless <version> is valid SemVer, greater than the latest tag, and
                                  tag `v<version>` does not exist yet. Prints the latest tag.
"""

import re
import subprocess
import sys

SEMVER = re.compile(r"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-([0-9A-Za-z-]+(?:\.[0-9A-Za-z-]+)*))?$")


def parse(version):
    match = SEMVER.match(version.strip().removeprefix("v"))
    if not match:
        return None
    major, minor, patch, pre = match.groups()
    return int(major), int(minor), int(patch), pre


def sort_key(version):
    major, minor, patch, pre = parse(version)
    if pre is None:
        return major, minor, patch, 1, ()
    # SemVer 2.0 §11: numeric identifiers sort below alphanumeric ones.
    ids = tuple((0, int(i), "") if i.isdigit() else (1, 0, i) for i in pre.split("."))
    return major, minor, patch, 0, ids


def latest_tag():
    tags = subprocess.run(["git", "tag", "-l", "v*"], check=True, capture_output=True, text=True).stdout.split()
    tags = [t for t in tags if parse(t)]
    return max(tags, key=sort_key) if tags else None


def fail(message):
    print(f"::error::{message}", file=sys.stderr)
    sys.exit(1)


def next_version(bumped):
    latest = latest_tag()
    if latest is None:
        fail("No v* tag found. Create the baseline tag first.")
    if parse(bumped) and sort_key(bumped) > sort_key(latest):
        return bumped.strip().removeprefix("v")
    major, minor, patch, pre = parse(latest)
    # A pre-release (3.0.0-rc.1) is followed by its release (3.0.0), anything else by the next patch.
    return f"{major}.{minor}.{patch}" if pre else f"{major}.{minor}.{patch + 1}"


def validate(version):
    if not parse(version) or version.startswith("v"):
        fail(f"'{version}' is not a valid SemVer version (expected e.g. 3.0.0 or 3.0.0-rc.1, without 'v').")
    latest = latest_tag()
    if latest and sort_key(version) <= sort_key(latest):
        fail(f"Version {version} is not greater than the latest tag {latest}.")
    exists = subprocess.run(["git", "rev-parse", "-q", "--verify", f"refs/tags/v{version}"], capture_output=True)
    if exists.returncode == 0:
        fail(f"Tag v{version} already exists.")
    return latest or ""


if __name__ == "__main__":
    if len(sys.argv) != 3 or sys.argv[1] not in ("next", "validate"):
        print(__doc__, file=sys.stderr)
        sys.exit(2)
    print(next_version(sys.argv[2]) if sys.argv[1] == "next" else validate(sys.argv[2]))
