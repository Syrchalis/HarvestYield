# HarvestYieldPatch
You think you know what the stat "Plant Harvest Yield" does? Well, let me surprise you - it does NOT increase the amount you get when you harvest plants, like "Mining Yield" does for ores.

It merely gives you a chance to fail below 100%, above 100% you do not gain anything.

This changes now! This small C# patch changes the method when harvesting plants so that below 100% nothing changes (you still have a chance to fail, but get 100% of the amount IF you succeed) - but above 100% you get more from the plant. If you have 177% plant harvest yield and the plant usually gives 10 berries, then you now get 17 or 18 (random).

Credit goes to Mehni and spd, potatoclip for helping me with C#.
