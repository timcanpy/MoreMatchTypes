using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MoreMatchTypes.Match_Setup
{
    public class MatchTypeHook
    {
        public static void ResetRules()
        {
            MoreMatchTypes_Form.moreMatchTypesForm.ResetModOptions();
        }

        public static void SetLuchaRules()
        {
            MoreMatchTypes_Form.moreMatchTypesForm.cb_luchaTag.Checked = true;

            if (Control.ModifierKeys == Keys.Shift)
            {
                MoreMatchTypes_Form.moreMatchTypesForm.cb_luchaFalls.Checked = true;
            }
            else
            {
                MoreMatchTypes_Form.moreMatchTypesForm.cb_luchaFalls.Checked = false;
            }
        }

        public static void SetEliminationRules()
        {
            MoreMatchTypes_Form.moreMatchTypesForm.cb_elimination.Checked = true;
        }

        public static void SetTTTRules()
        {
            MoreMatchTypes_Form.moreMatchTypesForm.cb_ttt.Checked = true;
        }

        #region Helper Methods
        private static void ShowMessage(String message)
        {
            MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
    }
}
