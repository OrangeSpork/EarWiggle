# EarWiggle

Adds Dynamic Bones to the Ears allowing Ears to Wiggle.

To use, enable on the character via the Ear Wiggle Menu (in the mod section at the bottom of the Face tab). Adjust settings as desired.

![Menu Option](https://raw.githubusercontent.com/OrangeSpork/EarWiggle/master/EarWiggle/UI.png)

![Controls](https://raw.githubusercontent.com/OrangeSpork/EarWiggle/master/EarWiggle/UIOptions.png)

Notes: The ear forks (base -> upper ear, base -> lower ear). Only the upper ear section is wired up in this initial version, might do the lower ear at some point, but it has some challenges (only a single bone, really).

On accessories: The accessory attachment point is at the earlobe, which isn't affected by this if you stick close. If you want accessories further up the ear conventional attachment won't work.

Advanced attachment to the upper ear isn't working as well as I'd hope (too much rotational variance?), still investigating.

For the moment you'll need to mesh attach accessories to the upper part of the ear.

Steps:

1) Parent to L or R ear as normal and move accessory to desired position.
![First](https://raw.githubusercontent.com/OrangeSpork/EarWiggle/master/EarWiggle/AdditonalAccessorySetting0.png)
2) Open advanced accessory window
![Next](https://raw.githubusercontent.com/OrangeSpork/EarWiggle/master/EarWiggle/AddtlAccessoryAttach.png)
3) Use the 'head' quick search option
4) Select the o_head bone
5) Select the 'Select Closest Vertex' to lock on to the closest vertex to where you have the accessory (by accessory center point).
6) Click Attach
7) Fine tune position and rotation to taste.
![Last](https://raw.githubusercontent.com/OrangeSpork/EarWiggle/master/EarWiggle/AdditionalAccessorySetting2.png)

To get it to track the ear motion you'll need to force head animation mode in the additional accessory plugin (recent version required).

![Plugin Settings](https://raw.githubusercontent.com/OrangeSpork/EarWiggle/master/EarWiggle/AccessorySettings.png)

Make sure Advanced Options is selected and check 'Allow for Head Bone Animation'. An update frequency of 1 will have best tracking at a (small) frame hit. Update frequency of 3 is fairly performance negligble but the accessory will drift a bit in fast motion.
