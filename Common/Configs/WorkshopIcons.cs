using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;

namespace WgMod.Common.Configs;

[Credit(ProjectRole.Programmer, Contributor.maimaichubs)]
public class WorkshopIconsConfig : ModConfig
{
    public static WorkshopIconsConfig Instance => ModContent.GetInstance<WorkshopIconsConfig>();
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [Header("WorkshopIcons")]
    [DefaultValue(1)]
    [Range(1, 1)]
    [Increment(1)]
    [Slider]
    [DrawTicks]
    public int WorkshopIcons;

    public string Description => WorkshopIcons switch
    {
        1 => "Art by @_d_u_m_m_y_",
        _ => "UwU"
    };

    [CustomModConfigItem(typeof(WorkshopIconsElement))]
    public string CurrentIcon => "WgMod/Assets/WorkshopIcons/WorkshopIcon" + WorkshopIcons;
}

[Credit(ProjectRole.Programmer, Contributor.follycake)]
public class WorkshopIconsElement : ConfigElement<string>
{
    UIImage _image;
    string _lastValue;

    public override void OnBind()
    {
        base.OnBind();
        _image = new UIImage(ModContent.Request<Texture2D>(Value))
        {
            MarginLeft = 30, // You can use this to move the fucking texture
            MarginTop = 0, // This too
            RemoveFloatingPointsFromDrawPosition = true
        };
        Append(_image);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (Value != _lastValue)
            _image.SetImage(ModContent.Request<Texture2D>(Value));
        _lastValue = Value;
    }
}