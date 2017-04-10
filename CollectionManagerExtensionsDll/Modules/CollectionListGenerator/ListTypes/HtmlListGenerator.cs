﻿using System.Collections.Generic;
using System.Text;
using CollectionManager.DataTypes;

namespace CollectionManagerExtensionsDll.Modules.CollectionListGenerator.ListTypes
{
    public class HtmlListGenerator : IListGenerator
    {
        private StringBuilder _mainStringBuilder = new StringBuilder();
        private StringBuilder _md5Output = new StringBuilder();

        /*{0} - username of creator
         * {1} - number of collections in list
         * {2} - Collections
         * 
         * 
        */
        private string _htmlOutputHeader = @"<!DOCTYPE html>
<html>
<head>
    <script src=""https://code.jquery.com/jquery-3.1.1.min.js"" type=""text/javascript""></script>
    <style type=""text/css"">
    body{{background:#a9a9ff;margin:0;padding:0;text-align:center;min-width:500px;}}a{{color:#264A7F;text-decoration:none;}}h1{{color:#B25C5C;font-size:20px;font-family:""Comic Sans MS"",cursive;padding:5px;margin:0px;}}
    </style>
    <title></title>
</head>
<body>
<script type=""text/javascript"">
$.fn.slideFadeToggle = function(speed, easing, callback) {{
    return this.animate({{
        opacity: 'toggle',
        height: 'toggle'
    }}, speed, easing, callback);
}};
</script>
<h1 style=""border-bottom: dotted;border-color:#422E1A"">List of maps in collections<br />
Generated by: ""{0}""<br />
Number of collections listed: {1}<br /></h1>";
        private string _htmlOutputFooter = "</body></html>";
        /*{0}-collection number
         * {1}-collection name
         * {2}-number of maps in collection(diffs)
         * 
         */
        string CollectionHeaderTemplate = @"<h1>Collection {0}: {1} ( {2} diffs )</h1>";
        /* {0} - map link
         * {1} - artist
         * {2} - title
         */
        string CollectionBeatmapTemplateFull = @"<a href=""{0}"" target=""_blank"">
{1} - {2} ";
        //{0} - md5
        string CollectionBeatmapTemplateMd5 = @"<a>No Data {0} </a><br />
";
        public void StartGenerating()
        {
            _mainStringBuilder.Clear();
        }

        public void EndGenerating()
        {
            _mainStringBuilder.Clear();
        }

        public string GetListHeader(Collections collections)
        {
            return string.Format(_htmlOutputHeader, "N/A", collections.Count);
        }

        public string GetCollectionBody(Collection collection, Dictionary<int, Beatmaps> mapSets, int collectionNumber)
        {
            _mainStringBuilder.Clear();

            _mainStringBuilder.AppendFormat(CollectionHeaderTemplate, collectionNumber,
                collection.Name, collection.NumberOfBeatmaps);

            foreach (var mapSet in mapSets)
            {
                GetMapSetList(mapSet.Key, mapSet.Value, ref _mainStringBuilder);
            }

            return _mainStringBuilder.ToString();
        }

        public string GetListFooter(Collections collections)
        {
            return _htmlOutputFooter;
        }
        private void GetMapSetList(int mapSetId, Beatmaps beatmaps, ref StringBuilder sb)
        {

            if (mapSetId == -1)
            {
                foreach (var map in beatmaps)
                {
                    if (map.MapId > 0)
                    {
                        sb.AppendFormat(CollectionBeatmapTemplateFull, map.MapLink, map.ArtistRoman,
                            map.TitleRoman);
                        if (!string.IsNullOrWhiteSpace(map.DiffName))
                            sb.AppendFormat("<a href=\"https://osu.ppy.sh/b/{0}\">[{1}]{2}★</a> ",
                                map.MapId, map.DiffName, map.StarsNomod);
                    }
                    else
                    {
                        _md5Output.AppendFormat(CollectionBeatmapTemplateMd5, map.Md5);
                    }
                }
            }
            else
            {
                sb.AppendFormat(CollectionBeatmapTemplateFull, beatmaps[0].MapLink, beatmaps[0].ArtistRoman,
                    beatmaps[0].TitleRoman);
                foreach (var map in beatmaps)
                {
                    sb.AppendFormat("<a href=\"https://osu.ppy.sh/b/{0}\">[{1}]{2}★</a> ",
                        map.MapId, map.DiffName, map.StarsNomod);

                }
                sb.Append("<br />");
            }
        }
    }
}