<p align="center">
    <img src="https://github.com/MahStudio/Winsta/raw/master/WinGoTag/Assets/Logos/perfectColor.png" width=80 height=80>

  <h3 align="center">Winsta</h3>

  <p align="center">
    Unofficial Instagram client for Windows 10 devices.
    <br>
    <a href="https://t.me/joinchat/DQwGRg9P42TzBSJgGOYoJw">Insiders chat</a>
  &middot;
    <a href="https://install.appcenter.ms/orgs/mahstudio-u5ev/apps/winsta/distribution_groups/insiders">Nightly builds</a>
    <br>
    <br>
  <a href="https://install.appcenter.ms/orgs/mahstudio-u5ev/apps/winsta/distribution_groups/insiders">
    <img src="https://build.appcenter.ms/v0.1/apps/24d698cf-df43-48bd-8300-404b9dc3854a/branches/master/badge">
    </a>
  <a href="https://www.paypal.me/mohsens22">
    <img src="https://img.shields.io/badge/Donate-Paypal-blue.svg" />
  </a>
  </p>

<br>

## Installation

1. First download our latest insider build from [App center](https://install.appcenter.ms/orgs/mahstudio-u5ev/apps/winsta/distribution_groups/insiders) or [Github release](https://github.com/MahStudio/Winsta/releases/latest).
2. Follow [these instructions](https://www.maketecheasier.com/install-appx-files-windows-10/) and install the appx package.

[DOWNLOAD CERTIFICATE](https://github.com/MahStudio/Winsta/raw/master/UnofficialInstagram_1.1.3.0_x86_ARM.cer)

### FAQ

> I have installation problems related to certificate.

Use [Colin Kiama's Easy Cert install](https://github.com/colinkiama/EasyCertInstall) and install the certificate.

Alternatively, you can download [FULL PACKAGE](https://github.com/MahStudio/Winsta/releases/latest) and right click on the `Add-AppDevPackage.ps1` script and click `Run with Powershell`. If Windows complained about running the scrip, write `Set-ExecutionPolicy Unrestricted` in the powershell to allow running external scripts.

> How should I install it on mobile?

Get the appx file and tap it on mobile File Explorer. It will be installed. If it didn't, download and extract full package use device portal to install the package with it's dependencies.

## Features
- Login (Two Factor login supported | Challenge require supported)
- Login with Facebook
- Logout
- Like / Dislike
- View posts (Image, Video, Carousel)
- View Comments
- View your own profile
- View users profiles
- View recent activities
- View Stories
- Search and Explore
- Direct messages
- Cinema mode
- Accept and Ignore friend requests
- Download posts (Available for Public profiles, Your own medias, and medias you are tagged in)
<p align="center">
    <img src="https://user-images.githubusercontent.com/22152065/46222594-b3c1d680-c35d-11e8-97eb-42f74111fc99.png">
<p/>


## Contributing

### Build Prerequisites

1. Windows 10
2. Visual Studio 2017 (latest build) with universal windows development features installed.
3. GIT for Windows ([install from here](http://gitforwindows.org/))
4. Knowledge about C#, Xaml, MVVM and Windows development.

### Insider chat on Telegram

Join [Insiders group on Telegram](https://t.me/joinchat/DQwGRg9P42TzBSJgGOYoJw) to stay tuned about the development of this application, exchanging feedback and the insider builds.

### InstaApi

This project uses [https://github.com/MahStudio/InstagramAPI](https://github.com/MahStudio/InstagramAPI).

## License
Copyright © 2017-2018 [Winsta Authors](https://github.com/Mahstudio/Winsta/graphs/contributors) // [Mah Studio](https://mahstudio.github.io).

If you want to create your own Instagram Client feel free to use our [private API NuGet](https://www.nuget.org/packages/InstagramUWPAPI)
 and feel free to use this source code as a sample code.
 
> **NOTE**: Winsta is not going to be a freeware and we decided to continue this project in a private repository.
