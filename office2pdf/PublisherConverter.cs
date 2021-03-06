﻿/**
 *  office2pdf command line PDF conversion for Office 2007/2010
 *  Copyright (C) 2011  Cognidox Ltd
 *  http://www.cognidox.com/opensource/
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MSCore = Microsoft.Office.Core;
using Microsoft.Office.Interop.Publisher;

namespace office2pdf {
    /// <summary>
    /// Handle conversion of Microsoft Publisher files
    /// </summary>
    class PublisherConverter : Converter {
        public static new Boolean Convert(String inputFile, String outputFile) {
            Microsoft.Office.Interop.Publisher.Application app;
            String tmpFile = null;
            try {
                app = new Microsoft.Office.Interop.Publisher.Application();
                app.Open(inputFile, false, false, PbSaveOptions.pbDoNotSaveChanges);

                // Try and avoid dialogs about versions
                tmpFile = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pub";
                app.ActiveDocument.SaveAs(tmpFile, PbFileFormat.pbFilePublication, false);
                app.ActiveDocument.ExportAsFixedFormat(PbFixedFormatType.pbFixedFormatTypePDF, outputFile, PbFixedFormatIntent.pbIntentStandard, true);
                app.ActiveDocument.Close();
                ((Microsoft.Office.Interop.Publisher._Application)app).Quit();
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            } finally {
                if (tmpFile != null) {
                    System.IO.File.Delete(tmpFile);
                }
                app = null;
            }
        }
    }
}
