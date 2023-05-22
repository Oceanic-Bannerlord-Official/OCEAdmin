<p align="center">
  <img src="https://i.gyazo.com/b3636d006d6c9867060dcd8a0199c8dd.png">
</p>


# OCEAdmin

**OCEAdmin is a full server suite for any Bannerlord server.**

It can operate in a lightweight state without the usage of the web api, solely acting on a singular Bannerlord server. It is recommended you use the API to secure all community servers by having synced admins and bans. There is also a customisable permission system within the web portal.

For multiplayer module developers, the included permissions/role system is easy to hook into.

Supported modules: Native, Sword & Musket

# Requirements

- Harmony for .NET Framework 4.7.2 (included)
- Bannerlord Dedicated Server (1.1.x)
- Windows or Linux

## Features

All gameplay features are configurable and togglable.

- Specialist limits for archers and cavalry.
- Cavalry dismount auto-admin. Non cavalry will be told to dismount and slain after a grace period.
- Rich admin and permission system. Admin chat included.
- Voice and text chat mutes.
- Local logging of player actions and events.
- Intuitive commands with echo feedback and in-game multiple player result handling.
- Custom web panel to manage global admins and bans across all your servers.

## Installation

1. Download the latest release for your Bannerlord version. (It's located on the right panel)
2. Drag and drop into your Bannerlord Dedicated Server's module folder.
3. Inside the OCEAdmin module there are default startup settings you can configure before your first start up. This is regenerated if deleted.
4. Make sure to include *OCEAdmin into your command options for startup.

## Credits
- Adolphus[^1].
- Mentalrob[^2]
- Horns[^3]
- Muz[^4].  



[^1]: Rework + specialists, autoadmin, permissions, web api, etc.
[^2]: Original creator of ChatCommands
[^3]: Fork developer of ChatCommands
[^4]: Testing and general assistance

