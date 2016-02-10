﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EdgeJs;
using Raml.Parser.Builders;
using Raml.Parser.Expressions;
using System.IO;

namespace Raml.Parser
{
    public class RamlParser
    {
        public async Task<RamlDocument> LoadAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("path");

            var path = Path.GetDirectoryName(filePath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar, ' ') 
                + (filePath.Contains(Path.DirectorySeparatorChar.ToString()) ? Path.DirectorySeparatorChar : Path.AltDirectorySeparatorChar);

            using(var sr = new StreamReader(filePath))
            {
                var content = await sr.ReadToEndAsync();
                var raml = await LoadRamlAsync(content, path);

                return raml;
            }
        }

	    public async Task<RamlDocument> LoadRamlAsync(string raml, string path)
		{
            if (string.IsNullOrWhiteSpace(raml))
                throw new ArgumentException("raml");

			var load = Edge.Func(@"
                return function (obj, callback) {
                    var raml = require('raml-parser');
                    raml.load(obj.content, obj.path).then( function(parsed) {
                        callback(null, parsed);
                    }, function(error) {
                        callback(null, 'Error parsing: ' + error);
                    });
                }
            ");

			var rawresult = await load(new { content = raml, path } );
			var error = rawresult as string;
			if (!string.IsNullOrWhiteSpace(error) && error.ToLowerInvariant().Contains("error"))
				throw new FormatException(error);

			var ramlDocument = new RamlBuilder().Build((IDictionary<string, object>)rawresult);
			
			return ramlDocument;
		}
    }
}
