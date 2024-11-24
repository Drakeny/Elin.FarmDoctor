**A set of quality-of-life improvements introducing new insights and tools to elevate your farming prowess, based on your Farming skill level.**

# Current Features

-   Displays how many growing stages crops have and how long until the crop grows to the next stage _if_ you have the minimum farming levels required. (Configurable)  
    ![Growth Stages](https://uc455a6fdbc20d38b8f541880188.dl.dropboxusercontent.com/cd/0/inline/Ce_z4Tt5J5iIqSXB6y0lr8g2s_YcuA1A09z8ozlaw-yquWzxtYe4bQG09g_NQcfWkwYXj8AWWFAf3Dl1wmD8cbQghCRS3r6keyMe2V04Wzj8bOYLFXSmXDey30n0-yXt5G4VYeZyi9azPkfjSJjq8gnW/file#)

-   Ability to trigger the regrowth of produce for trees (Apples, Pears, Bananas, and Oranges) by re-applying fertilizer after the tree has been harvested. (Configurable)  
    ![Regrowth Example](https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExenpzeGw5cnI2NTRxMG92NXVicWhoYnoyMGNuMDU1enVpeXo0b2ZsMiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/8nPRa6w19l1eoJhjV7/giphy.gif)

-   Ability to inspect already growing crops (seed info) using the “Examine” keybind.  
    ![Crop Inspection](https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExM29jNGYwbXUzeWE3bHBmdGxsd3l5eG04a3Rzd29tdjd0am1vcHE0MyZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/MR7WfMlPBdaU0jGwiH/giphy.gif)

### Configuration

**Reminder:** For all mods that have configurable options, their config files are located at: `(steam)\Elin\BepInEx\config`. You need to open the game once with the mod enabled to create the config files.

**Crop Information**  
Default value for Basic Information (Growth stages) is 10. `[minFarmingLevelBasicInfo]`  
Default value for Full Information (Time to grow) is 17. `[minFarmingLevelFullInfo]`

**Regrowth**  
Regrowth can be enabled/disabled, and it is enabled by default. `[regrowth]`  
Default value to trigger regrowth is 15. `[minLevelRegrowth]`

### TODOs and POSSIBLE additional features

-   Figure out how to deal with seeds. (Right now, they don’t have growing info.)
-   Expand the farming possibilities.

---

# Changelog

-   **11/24/24**

    -   Regrowth feature left experimental stage and now is enabled by default.
    -   Added crop inspection/examine feature.

-   **11/21/24**

    -   Fixed mistakes because I dumb. (You can now actually trigger regrowth when above the minimum level required.)

-   **11/19/24**

    -   Added Experimental feature for regrowth of trees.

-   **11/18/24**
    -   Fixed calculation for trees.
    -   Fixed calculation for watered plants.
