
# Nitrokey Keepass Plugin

This is an abandoned project, which contains implementation of the PKCS#11 support for the [Keepass2] v2.22. While it might not work currently, it could be used as a starting point for the development in other projects.

For end-user, instead of this one, please use other plugin for protecting the password database - [OtpKeyProv]. See this [comment] for details, which links to this [guide].

[comment]: https://github.com/Nitrokey/nitrokey-keepass-plugin/issues/2#issuecomment-293904422
[guide]: https://www.nitrokey.com/documentation/applications#a:password-manager
[OtpKeyProv]: https://keepass.info/plugins.html#otpkeyprov


|Warning|
|---|
|This repository provides Keepass2 binary in version v2.22. Please keep in mind, that current version is v2.41. See official [Keepass2] page for details.|

[Keepass2]: https://keepass.info/

## Current state
Whether plugin currently works is not confirmed - to test on Windows. 

On Fedora Linux 29 the provided Keepass binary successfully launches (via Mono), and finds the plugin. The latter expects a `Pkcs11Interop` library in a `.so` format, while official package distributes only `.dll`, so further tests are not possible on this OS.

Provided Keepass binary does not work correctly, when launched with Wine 4.0.


## License
Project is licensed under GPLv3. It contains an old version of Pkcs11Interop, and this particular one is licensed under AGPL - see [Pkcs11Interop] directory for the details, and [official Pkcs11Interop site].

[Pkcs11Interop]: ./Pkcs11Interop/
[official Pkcs11Interop site]: https://www.pkcs11interop.net/