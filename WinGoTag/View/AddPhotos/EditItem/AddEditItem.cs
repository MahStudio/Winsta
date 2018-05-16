using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinGoTag.View.AddPhotos.EditItem
{
    public class AddEditItem
    {
        public ObservableCollection<GridViewEditItem> HamburgerMenuItems =
             new ObservableCollection<GridViewEditItem>();
        //                          Adjust    Bright    Contrast           saturion    Color     Fade
        private string[] icons = { "\ue78A", "\ue706", "\ue793", "\ue7BA", "\ueB42", "\ueF3C", "\ue753" };

        private string[] texts = { "Adjust", "Brightness", "Contrast", "Warmth", "Saturation", "Color", "Fade", "Highlighths", "Shadows", "Vignette", "Tilt Shift", "Sharpen" };

        private Type[] frames = { null, null, null, null };

        public AddEditItem()
        {
            for (int i = 0; i < icons.Length; i++)
            {
                HamburgerMenuItems.Add(new GridViewEditItem()
                    {

                        Icon = icons[i],
                        Text = texts[i],
                        //TargetFrame = frames[i]
                    });
            }
        }
    }
}
