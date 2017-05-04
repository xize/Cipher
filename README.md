# Cipher plugin for keepass
this is a separated branch for keepass plugin developement for Cipher.

#### Why an Keepass plugin?
Although Keepass is a very strong password manager, we believe it doesn't serve any security after an attacker managed to compromise the database.

One of the most common mistakes people seem to make is that they also host their OTP(One Time Password) secrets inside the database.
Once a attacker is able to manage your two steps authentication, it means you lost your identity if they are able to get inside your primary mail.

To battle this problem we thought about a solution how to combat this scenario.

If you are able to decipher your OTP secrets with a secret phrase and the OTP two factor authentication is enabled on your primary email you can safe alot of accounts because this phrase is something you have inside your head.
and maybe you only need to use the phrase once in a year and scan it on your new device via QR ;-)