# Cipher plugin for keepass
this is a separated branch for keepass plugin developement for Cipher.

#### Why an Keepass plugin?
Although Keepass is a very strong password manager, we believe it doesn't serve any security after an attacker managed to compromise the database.

One of the most common mistakes people seem to make is that they also host their OTP(One Time Password) secrets inside the database.    
Once a attacker is able to manage your two steps authentication, it means you lost your identity if they are able to get inside your primary mail.

To battle this problem we thought about a solution how to combat this scenario.

If you are able to decipher your OTP secrets with a secret phrase and the OTP two factor authentication is enabled on your primary email you can safe alot of accounts because this phrase is something you have inside your head.  
and maybe you only need to use the phrase once in a year and scan it on your new device via QR ;-)

#### What is this plugin going todo?

As for now this plugin does not serve much of a purpose since it only opens the Cipher executable via the tools menu in keepass.

But we are planning to add a couple of features such as:

- adding a new button inside the entry context menu(the menu when you right click to create entries):    
when a user has selected any entry and right clicks it and presses this new button he is able to write a cipher this cipher will replace the password field in keepass.

- adding a more safer way to recover in case the phrase was written wrong:    
when a user is inside this wizard to write a cipher, the plugin will automaticly make a copy from the vanilla database.    
when the user has filled in his secret phrase he will be asked to decipher it, if the deciphering fails the vanilla database will be placed back.

- adding a way to cipher a collection of entries

- add prevention against double ciphering