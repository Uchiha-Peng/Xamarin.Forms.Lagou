using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using Lagou.Droid.Renders;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageRender))]
namespace Lagou.Droid.Renders {
    public class TabbedPageRender : TabbedPageRenderer {

        private Android.Views.View formViewPager = null;
        private TabLayout tab = null;

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e) {
            base.OnElementChanged(e);

            this.formViewPager = this.GetChildAt(0);
            this.tab = (TabLayout)this.GetChildAt(1);

            this.UpdateTabIcons();
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b) {
            base.OnLayout(changed, l, t, r, b);

            // update layout , let tab on the bottom of the page
            // formViewPager upon tab.
            var w = r - 1;
            var h = b - t;
            if (w > 0 && h > 0) {
                int ypos = Math.Min(h, Math.Max(this.tab.MeasuredHeight, this.tab.MinimumHeight));
                this.formViewPager.Layout(0, -ypos, r, b - ypos);
                this.tab.Layout(l, h - ypos, r, b);
            }
        }


        private void UpdateTabIcons() {
            var tabLayout = this.tab;
            if (tabLayout.TabCount != this.Element.Children.Count)
                return;

            var tdb = new TextDrawableBuilder(this.Context);
            //tdb.SetColor(Color.Accent.ToAndroid());
            var arr = this.Context.ObtainStyledAttributes(new int[] { Resource.Attribute.colorAccent });
            var color = arr.GetColor(0, 0);
            tdb.SetColor(color);

            for (int i = 0; i < this.Element.Children.Count; ++i) {
                var page = this.Element.Children[i];
                if (string.IsNullOrEmpty(page.Icon)) {

                    var glyph = (string)page.GetValue(AttachedFontIcon.GlyphProperty);
                    if (!string.IsNullOrWhiteSpace(glyph)) {
                        tdb.SetText(glyph);

                        var font = (string)page.GetValue(AttachedFontIcon.FontFamilyProperty);
                        if (!string.IsNullOrWhiteSpace(font))
                            tdb.SetFont(font.ToTypeface());

                        var size = (int)(double)page.GetValue(AttachedFontIcon.FontSizeProperty);
                        tdb.SetSize(size);

                        tabLayout.GetTabAt(i)
                            .SetIcon(tdb.Build());
                    }
                }
            }
        }

        internal static class MeasureSpecFactory {
            public static int MakeMeasureSpec(int size, MeasureSpecMode mode) {
                return size + (int)mode;
            }

            //public static int GetSize(int measureSpec) {
            //    return measureSpec & 1073741823;
            //}
        }
    }
}