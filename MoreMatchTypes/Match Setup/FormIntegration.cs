using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DG;

namespace MoreMatchTypes.Match_Setup
{
    class FormIntegration
    {
        [ControlPanel(Group = "MoreMatchTypes")]
        public static Form MSForm()
        {
            if (MoreMatchTypes_Form.moreMatchTypesForm == null)
            {
                return new MoreMatchTypes_Form();
            }
            {
                return null;
            }
        }
    }
}
