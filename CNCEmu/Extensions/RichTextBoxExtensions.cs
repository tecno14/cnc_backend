using System;
using System.Windows.Forms;

namespace CNCEmu.Extensions
{
    public static class RichTextBoxExtensions
    {
        public static void AppendLog(this RichTextBox box, Utils.Logger.Log log)
        {
            if (box.InvokeRequired)
            {
                box.Invoke(new Action(() => box.AppendLog(log)));
                return;
            }

            bool isAtBottom = box.IsAtBottom();

            // Append the text with the specified color
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = log.Type.GetColor();
            box.AppendText(log + Environment.NewLine);
            box.SelectionColor = box.ForeColor; // Reset to default color

            // Scroll to the end if it was at the bottom before appending
            if (isAtBottom)
                box.ScrollToEnd();
        }

        /// <summary>
        /// Check if the vertical scrollbar is at the bottom
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        private static bool IsAtBottom(this RichTextBox box)
        {
            return box.GetPositionFromCharIndex(box.TextLength - 1).Y <= box.ClientSize.Height;
        }

        private static void ScrollToEnd(this RichTextBox box)
        {
            box.SelectionStart = box.TextLength;
            box.ScrollToCaret();
        }
    }
}
