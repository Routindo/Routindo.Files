using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Routindo.Plugins.Files.UI.ViewModels;

namespace Routindo.Plugins.Files.UI.Views
{
    public class DeleteByPatternView: FilesSelectorConfigurator
    {
        public DeleteByPatternView()
        {
            this.DataContext = new DeleteByPatternViewModel();
        }
    }
}
