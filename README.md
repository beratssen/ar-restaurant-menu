<div align="center">

# AR Restaurant Menu

**English** · [Türkçe](README.tr.md)

Point your phone at the menu and the dish appears in 3D on the table.

<table>
  <tr>
    <td align="center"><img src="docs/turntable-pirzola.gif" width="240" alt="Pork chop and fries" /><br><sub>Pork chop &amp; fries</sub></td>
    <td align="center"><img src="docs/turntable-makarna.gif" width="240" alt="Pasta" /><br><sub>Pasta</sub></td>
    <td align="center"><img src="docs/turntable-tost.gif" width="240" alt="Toasted sandwich" /><br><sub>Toasted sandwich</sub></td>
  </tr>
</table>

![Unity](https://img.shields.io/badge/Unity_2022.3_LTS-000000?style=flat-square&logo=unity&logoColor=white)
![Vuforia](https://img.shields.io/badge/Vuforia-0F9D58?style=flat-square)
![C#](https://img.shields.io/badge/C%23-512BD4?style=flat-square&logo=csharp&logoColor=white)
![Blender](https://img.shields.io/badge/Blender-E87D0D?style=flat-square&logo=blender&logoColor=white)
![Figma](https://img.shields.io/badge/Figma-F24E1E?style=flat-square&logo=figma&logoColor=white)
![Android](https://img.shields.io/badge/Android-3DDC84?style=flat-square&logo=android&logoColor=white)

</div>

## The idea

A flat photo on a paper menu tells you little about a dish: how big the portion is, how it is plated, what is on it. This app lets a guest scan a QR code on the menu with their phone camera and look at a true-to-life 3D model of the dish in augmented reality *before* ordering.

## How it works

1. **Photograph** each dish: about 100 photos around the plate from several heights (iPhone 11, indoor light).
2. **Reconstruct** it with photogrammetry. I compared Kiri Engine and RealityScan on the same dish; RealityScan gave more detail, so all final models use it.
3. **Clean up in Blender.** Photogrammetry also captures the table and the surroundings, so I removed the stray geometry by hand, repaired broken mesh regions, and fixed UV problems by re-applying UV coordinates from the healthy areas.
4. **Optimise for mobile.** The first Android build stuttered because some models had around 3 million polygons. With Blender's Decimate modifier I brought them down to roughly 50,000 and cut textures from 4K to 2K. Frame rate recovered.
5. **Track the marker** with Vuforia. The marker is a QR code generated with Python. Vuforia's Target Manager rated it 5 stars for detectability, the same rating my first Figma-designed menu image got.
6. **Build the interface** in Figma and rebuild it in Unity on a 1920×1080 canvas. Categories at the top switch between dishes.

## Stack

| Layer | Tools |
| --- | --- |
| Engine / AR | Unity 2022.3 LTS, Vuforia Engine 11.4 (image target) |
| Language | C# |
| 3D capture | iPhone 11, RealityScan, Kiri Engine |
| 3D cleanup | Blender: mesh editing, UV mapping, Decimate |
| UI design | Figma |
| Marker | QR code generated in Python |
| Target | Android (APK) |

## What I learned

- Photogrammetry output is a starting point. The mesh and UV cleanup in Blender decided whether a model looked right.
- Mobile AR is a performance budget problem: polygon count and texture size matter as much as visual quality.
- Marker-based AR is dependable, but it needs the marker in view. A markerless version is a natural next step.

## Possible next steps

- Markerless AR (plane detection) so no QR code is needed
- More dishes and multi-language menus
- Ordering integration

## The menu logic

`Assets/AR_Menu_Manager.cs` drives the on-screen menu. Dishes are plain data (model, photo, name, description, price) grouped into categories and edited in the Inspector. The manager shows one dish at a time, switches category, cycles with next/previous, highlights the active category with a smoothly sliding underline, and updates the pager dots.

## Run it yourself

1. Open the project with **Unity 2022.3 LTS**.
2. The Vuforia Engine package (11.4.4) is not included. Download it from the [Vuforia developer portal](https://developer.vuforia.com/) and add it through *Package Manager > Add package from tarball*.
3. Create a free Vuforia license key and paste it in *Vuforia Configuration*. The key is deliberately not in this repository.
4. Upload your own QR code as an image target in Vuforia Target Manager, assign the database to the ImageTarget, and build for Android.

## Repository layout

```
Assets/            scripts, 3D models and textures, UI sprites, scenes
Packages/          package manifest
ProjectSettings/   Unity project settings
docs/              README previews
```
