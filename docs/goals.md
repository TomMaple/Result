# Goals
1. Follow standards where possible:
    1. Industry standard for error responses: [RFC 9457](https://datatracker.ietf.org/doc/html/rfc9457) (which updates the [RFC 7807](https://datatracker.ietf.org/doc/html/rfc7807))
    1. Error types: existing *Result* implementations use the same or similar type of errors.
    1. Naming: using idiomatic and clear names.
1. Easy to use:
	1. Easy API, idiomatic, with easy to guess usage, that does not require studying documentation.
	1. Limited number of required parameters, data.
	1. Limited number of additional required code (e.g., additional classes, enums, records, etc.).
1. Flexible:
	1. Easy to extend—without writing additional code.
	1. Easy to extend—by writing additional code.
	1. Supports i18n.
1. Minimize code degradation:
	1. Well-organized.
	1. Consistent.
	1. Supporting standard scenarious so there is no need to “reinvent the wheel”.
