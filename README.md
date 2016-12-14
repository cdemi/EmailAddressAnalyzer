# EmailAddressAnalyzer

Testing a proof of concept of higher quality email verification.

Instead of doing a regex check, parse the domain part of the email, check if the domain even exists. If the domain exists, check if email is setup on the domain