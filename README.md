# Result
An abstraction which contains the value or the error of the operation. It can be mapped to the HTTP reponse if needed.

# Project status: EARLY STAGE

# Error Response
Example
```json
{
    "type": "https://example.com/probs/out-of-credit", 
    "status": 400,
    "title": "You do not have enough credit.",
    "detail": "Your current balance is 30, but that costs 50.",
    "instance": "/accounts/12345/msgs/abc",
    "errors": [
        {
            "pointer": "#/age",
            "detail": "must be a positive integer",
            "detailTemplate": {
                "messageId":"user.details.age.mustBePositive"
            }
        },
        {
            "pointer": "#/profile/colour",
            "detail": "must be ‘green’, ‘red’ or ‘blue’",
            "detailTemplate": {
                "messageId": "user.profile.colour",
                "params": {
                    "validValueIds": [
                        "user.profile.colour.green",
                        "user.profile.colour.red",
                        "user.profile.colour.blue"
                    ]
                }
            }
        }
    ],
    "detailTemplate": {
        "messageId": "user.account.balance.tooLow",
        "params": {
            "errorCode": "UAB17",
            "accounts": [
                {
                    "title": "Main (***9456)",
                    "url": "/accounts/12345"
                },
                {
                    "title": "Main (***3357)",
                    "url": "/accounts/67890"
                }
            ],
            "currentBalance": 30,
            "requiredBalance": 50
        }
    },
    "location": "252e0b7f-b21c-44ab-bda0-c72fe943b886"
}
```