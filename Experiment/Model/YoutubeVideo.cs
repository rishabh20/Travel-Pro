using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experiment.Model
{
        public class YoutubeVideo
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public DateTime PubDate { get; set; }
            public Uri YoutubeLink { get; set; }
            public Uri VideoLink { get; set; }
            public Uri Thumbnail { get; set; }
            public string Description { get; set; }
        }

}
