using QTChinnok.Logic.Entities.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTChinnok.Logic.Controllers.App
{
    partial class MusicCollectionsController
    {
        internal override IEnumerable<string> Includes => new[] { nameof(MusicCollection.Albums)};
    }
}
