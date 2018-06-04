using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace WinGoTag.Helpers
{
    public class PassageHelper : IDisposable
    {
        public Paragraph GetParagraph(string text, TypedEventHandler<Hyperlink, HyperlinkClickEventArgs> hyperLinkAction, Color? defaultColor = null, Color? hyperColor = null)
        {
            Paragraph p = new Paragraph();
            if (hyperColor == null)
                hyperColor = Colors.DodgerBlue;
            if (defaultColor == null)
                defaultColor = Colors.Black;
            try
            {
                var splitter = new List<string>(text.Split(new[] { "\r\n", Environment.NewLine, "\n", "\r" }, StringSplitOptions.None));

                var replacedText = string.Join(" @@@@@ ", splitter);
                var parsedText = GetParsedPassage(replacedText);

                foreach (var item in parsedText)
                {
                    if (item.PassageType == PassageType.Text)
                    {
                        if (item.Text.Contains("@") || item.Text.Contains("#") || item.Text.Contains("http://") ||
                            item.Text.Contains("https://") || item.Text.Contains("www."))
                        {
                            var hyper = new Hyperlink
                            {
                                Foreground = new SolidColorBrush(hyperColor.Value)
                            };
                            hyper.Inlines.Add(new Run() { Text = item.Text, Foreground = new SolidColorBrush(hyperColor.Value) });
                            hyper.Click += hyperLinkAction;
                            p.Inlines.Add(hyper);
                            p.Inlines.Add(new Run() { Text = " " });
                        }
                        else
                            p.Inlines.Add(new Run() { Text = item.Text + " ", Foreground = new SolidColorBrush(defaultColor.Value) });
                    }
                    else
                    {
                        if (item.Text.StartsWith("@@@@@"))
                            p.Inlines.Add(new LineBreak());
                        else
                        {
                            var hyper = new Hyperlink
                            {
                                Foreground = new SolidColorBrush(hyperColor.Value)
                            };
                            hyper.Inlines.Add(new Run()
                            {
                                Text = item.Text,
                                Foreground = new SolidColorBrush(hyperColor.Value),

                            });
                            hyper.Click += hyperLinkAction;
                            p.Inlines.Add(hyper);
                            p.Inlines.Add(new Run() { Text = " " });
                        }
                    }
                }
            }
            catch
            {
                p.Inlines.Clear();
                p.Inlines.Add(new Run() { Text = text, Foreground = new SolidColorBrush(defaultColor.Value) });
            }
            return p;
        }
        public List<Passage> GetParsedPassage(string text)
        {
            var parsedList = new List<Passage>();
            try
            {
                var list = new List<string>(text.Split(new string[] { " ", Environment.NewLine }, StringSplitOptions.None));
                foreach (var item in list)
                {
                    if (item.StartsWith("http://") || item.StartsWith("https://") || item.StartsWith("www."))
                        parsedList.Add(new Passage { Text = item, PassageType = PassageType.Url });
                    else if (item.StartsWith("#"))
                        parsedList.Add(new Passage { Text = item, PassageType = PassageType.Hashtag });
                    else if (item.StartsWith("@"))
                        parsedList.Add(new Passage { Text = item, PassageType = PassageType.User });
                    else
                    {
                        parsedList.Add(new Passage { Text = item, PassageType = PassageType.Text });
                    }
                }
            }
            catch { }

            return parsedList;
        }
        
        bool disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            //if (disposing)
            //{
            //}
            disposed = true;
        }
    }
    public class Passage
    {
        public string Text { get; set; }
        public PassageType PassageType { get; set; }
    }
    public enum PassageType
    {
        Text = -1,
        Hashtag,
        Url,
        User
    }
}
