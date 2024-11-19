**A set of quality-of-life improvements introducing new insights and tools to elevate your farming prowess, based on your Farming skill level.**

### Current Features

-   Displays how many growing stages crops have.
-   Displays how long until the crop grows to the next stage **IF** you have the minimum farming level required.
-   Configurable minimum farming level required.
    -   Defaults are level 10 for Basic info (Growth stages) and 17 for full info (Time to grow).
-   **EXPERIMENTAL**: Ability to trigger the regrowth of produce for trees (Apples, Pears, Bananas, and Oranges) by re-applying fertilizer after the tree has been harvested.
    -   As an experimental feature, this is disabled by default. You can enable and configure the minimum farming level needed by editing the config file at `(steam)\Elin\BepInEx\config`.

**Reminder:** For all mods that have configurable options, their config files are located at: `(steam)\Elin\BepInEx\config`.

---

### TODOs and POSSIBLE additional features

**Again, this is tentative.**

-   Figure out how to deal with seeds. (Right now they don't have growing info.)
-   Add extra inspect window for even more detailed information. (What information? IDK, some information.)
-   **POSSIBLY** expand the grow system. (This one warrants the big "possibly" because in the end, this is EA, and unused systems most likely are planned to be expanded upon by Noa.)

---

### Changelog

-   **11/19/24**

    -   Added Experimental feature for regrowth of trees.

-   **11/18/24**
    -   Fixed calculation for trees.
    -   Fixed calculation for watered plants.
