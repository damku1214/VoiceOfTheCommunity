# Voice of the Community
This is a mod that adds new custom items to the game, all from the ideas of the Official Skul Discord!

## PLEASE BE AWARE!!!
For those who wonder, using the English and Korean version together will not malfunction, one of them will get negated.

This project is a variation of MrBacanudo's [Custom Items mod](https://github.com/MrBacanudo/SkulHardModeMods/tree/main/CustomItems).
None of the code belongs to me excluding content related to custom items.

## Content
This mod contains:

* 6 Commons
* 20 Rares
* 25 Uniques
* 9 Omens
* 31 Legendaries

91 items in total! (FYI vanilla has about 240 items in total)

## Thunderstore
The mod is now published on Thunderstore! Go and check it out!

## Manual Installation
BE AWARE that this installation method is ripped off directly from [MrBacanudo's SkulHardModeMods](https://github.com/MrBacanudo/SkulHardModeMods/tree/main).

1. Download the external dependencies
    * BepInEx (Windows x64): https://github.com/BepInEx/BepInEx/releases/download/v5.4.21/BepInEx_x64_5.4.21.0.zip
    * Unstripped Core Libraries: https://unity.bepinex.dev/corlibs/2020.3.34.zip
    * Unstripped Unity Libraries: https://unity.bepinex.dev/libraries/2020.3.34.zip
2. Extract all files into the Skul folder
    * Default is `C:\Program Files (x86)\Steam\steamapps\common\Skul` or `C:\Program Files\Steam\steamapps\common\Skul`
      * You can find this by right clicking Skul on Steam -> Properties -> Local Files -> Browse
    * BepInEx must be extracted to the game's root folder (i.e. you must see `winhttp.dll` and `doorstop_config.ini` in that folder)
    * The other two must be extracted inside the `2020.3.34` folder (the same folder!)
3. Edit the new `doorstop_config.ini` file inside the game folder:
    * Change the last line from `dllSearchPathOverride=` to `dllSearchPathOverride=2020.3.34`
4. Download the mods! Latest one here: https://github.com/damku1214/VoiceOfTheCommunity/releases/latest
5. Put the mods you want into the `BepInEx/plugins` folder inside your game folder
    * You're allowed to mix or just use what you want!

Please keep in mind, the mod doesn't gurantee that your game will be 100% fine.
Use this at your own risk. Here be dragons!

## Contributors
I would like to give a huge thanks to people who contributed to this mod. This includes:
* iBearATK - creator of 'Vase of the Fallen', 'Broken Heart', 'Lustful Heart', 'Small Twig', 'Volcanic Shard', 'Shrinking Potion', 'Growing Potion', 'Unstable Size Potion', 'Lucky Coin', 'Soul Flame Scythe', 'Omen: Cursed Shield', 'Bottled Faeling', 'Gazing Eye Brooch', 'Gryphon's Feather', 'Shadow Thief's Sack', 'Monk's Bracers', and 'Monk's Tiger Claw Bracers'
* #1 swap enjoyer - creator of 'Rusty Chalice', 'Goddess's Chalice', 'Blood-Soaked Javelin', 'Cross Necklace', 'Mana Accelerator', 'Fonias', 'Omen: Accursed Sabre', 'Disorientation Device', 'Carleon Commander's Bihänder', 'Demon-guard's Training Sword', 'Shield of the Unrelenting', 'Omen: Horcrux Pendant', 'Standard-issue Mining Pick', 'Dwarven Legend's Pickaxe', 'Sagittarius's Chakram', 'Blooming Eden', 'Steel Aegis', 'B0n3 0f R4nD0mn3ss', 'B0n3 0f 3t3rn1ty: Randomness', 'Power Halberd', 'Demoman's Bottle', 'Forgotten Company Helmet', and 'Manatech Sequence Breaker'
* Haxa - creator of 'Omen: Flask of Botulism', 'Omen: Corrupted Symbol', 'Beginner's Lance', 'Heavy-Duty Carleon Helmet', 'Ginga Pachinko', 'Kabuto', and 'Mask of Sogeking'
* Chained Champion - creator of 'Tainted Finger', 'Recoverd Fingers', 'Corrupted God's Hand', 'Rotten Wings', 'Mana Fountain', 'Attendant's Cuirass', 'Omen: Cursed Daggers', and 'The Endless Cycle'
* Steak - creator of 'Dream Catcher', 'Winged Spear', 'Solar-Winged Sword', 'Lunar-Winged Insignia', 'Wings of Dawn', 'Omen: Last Dawn', 'Weird Herbs', and 'Devil's Mask'
* Vaalfen - creator of 'Frozen Spear', 'Spear of the Frozen Moon', 'Sword of the Toxic Moon', and 'Scythe of the Twin Moons'
* viktor_k - creator of 'Spiky Rapida', 'Shieldbone', 'Bone of Eternity: Shield', 'Diary of an Old Master', and 'Omen: The Secret of the King'
* WillSP - creator of 'Cursed Hourglass', 'Rusty Shovel', 'Crimson Cap', 'Sword of Ages', 'Broken Watch', 'Helix Brooch', 'Tetronimo', 'T-Bone', 'Craftsman's Chisel', 'Master Craftsman's Chisel', and 'Makeshift Helmet'
* נתנאל יוסופוב - creator of 'The Sword of the Protector'
* lwOythgiMylS - creator of 'Becchi', 'Happiest Mask', and 'Omen: Damocles'
* BlazingSun - creator of 'Ring Target'
* damku1214 (Yes, it's me!) - creator of 'Tainted Red Scarf', 'Tattered Cat Plushie', and 'Golden Megaphone'

## For Developers
The freeze max hit increase by the 'Spear of the Frozen Moon' is hard-coded due to game limitations,
which means that instead of just increasing the max hit count, it sets the hit count depending on the number of said item you have
and the number of Absolute Zero inscriptions you have.

The jump height increase by the 'Gryphon's Feather' is also hard-coded into the game; it may have conflicts with future
mods that contain items that increase jump height.

The bone item randomizer from the 'B0n3 0f R4nD0mn3ss' is also semi-hard coded. It will have conflicts if the bone item naming
do not match the naming rules by MrBacanudo and I.

The hazardous item pool from the 'Crimson Cap' is hard-coded as well.

If you want to make an item mod that configures hard-coded features and want it compatible with this mod,
please contact me via Discord (damku1214) and we can sort stuff out.