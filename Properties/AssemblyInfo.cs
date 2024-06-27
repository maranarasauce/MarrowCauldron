[assembly: AssemblyTitle(MarrowCauldron.Main.Description)]
[assembly: AssemblyDescription(MarrowCauldron.Main.Description)]
[assembly: AssemblyCompany(MarrowCauldron.Main.Company)]
[assembly: AssemblyProduct(MarrowCauldron.Main.Name)]
[assembly: AssemblyCopyright("Developed by " + MarrowCauldron.Main.Author)]
[assembly: AssemblyTrademark(MarrowCauldron.Main.Company)]
[assembly: AssemblyVersion(MarrowCauldron.Main.Version)]
[assembly: AssemblyFileVersion(MarrowCauldron.Main.Version)]
[assembly:
    MelonInfo(typeof(MarrowCauldron.Main), MarrowCauldron.Main.Name, MarrowCauldron.Main.Version,
        MarrowCauldron.Main.Author, MarrowCauldron.Main.DownloadLink)]
[assembly: MelonColor(255, 255, 255, 255)]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONELAB")]