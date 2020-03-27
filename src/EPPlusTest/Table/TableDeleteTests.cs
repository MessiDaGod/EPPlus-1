/*******************************************************************************
 * You may amend and distribute as you like, but don't remove this header!
 *
 * Required Notice: Copyright (C) EPPlus Software AB. 
 * https://epplussoftware.com
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.

 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 * See the GNU Lesser General Public License for more details.
 *
 * The GNU Lesser General Public License can be viewed at http://www.opensource.org/licenses/lgpl-license.php
 * If you unfamiliar with this license or have questions about it, here is an http://www.gnu.org/licenses/gpl-faq.html
 *
 * All code and executables are provided "" as is "" with no warranty either express or implied. 
 * The author accepts no liability for any damage or loss of business that this product may cause.
 *
 * Code change notes:
 * 
  Date               Author                       Change
 *******************************************************************************
  01/27/2020         EPPlus Software AB       Initial release EPPlus 5
 *******************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
namespace EPPlusTest.Table
{
    [TestClass]
    public class TableDeleteTests : TestBase
    {
        static ExcelPackage _pck;
        [ClassInitialize]
        public static void Init(TestContext context)
        {
            InitBase();
            _pck = OpenPackage("TableDelete.xlsx", true);
        }
        [ClassCleanup]
        public static void Cleanup()
        {
            SaveAndCleanup(_pck);
        }
        #region Delete Row
        [TestMethod]
        public void TableInsertRowTop()
        {
            //Setup
            var ws = _pck.Workbook.Worksheets.Add("TableDeleteTop");
            LoadTestdata(ws, 100);

            var tbl = ws.Tables.Add(ws.Cells["A1:D100"], "TableDeleteTop");
            ws.Cells["A102"].Value = "Shift Me Up";
            tbl.DeleteRow(0);

            Assert.AreEqual("A1:D99", tbl.Address.Address);
            Assert.AreEqual(3, ws.Cells["B2"].Value);
            Assert.AreEqual("Shift Me Up", ws.Cells["A101"].Value);
            tbl.DeleteRow(0, 3);
            Assert.AreEqual("Shift Me Up", ws.Cells["A98"].Value);
        }
        [TestMethod]
        public void TableDeleteRowBottom()
        {
            //Setup
            var ws = _pck.Workbook.Worksheets.Add("TableDeleteBottom");
            LoadTestdata(ws, 100);
            ws.Cells["A102"].Value = "Shift Me Up";
            var tbl = ws.Tables.Add(ws.Cells["A1:D100"], "TableDeleteBottom");
            tbl.DeleteRow(98, 1);
            Assert.AreEqual("A1:D99", tbl.Address.Address);
            Assert.AreEqual(99, ws.Cells["B99"].Value);
            Assert.AreEqual("Shift Me Up", ws.Cells["A101"].Value);
            tbl.DeleteRow(95, 3);
            Assert.AreEqual("A1:D96", tbl.Address.Address);
            Assert.AreEqual("Shift Me Up", ws.Cells["A98"].Value);
        }
        [TestMethod]
        public void TableDeleteRowBottomWithTotal()
        {
            //Setup
            var ws = _pck.Workbook.Worksheets.Add("TableDeleteBottomTotal");
            LoadTestdata(ws, 100, 2);
            ws.Cells["B102"].Value = "Shift Me Up";
            ws.Cells["F5"].Value = "Don't Shift Me";

            var tbl = ws.Tables.Add(ws.Cells["B1:E100"], "TableDeleteBottomTotal");
            tbl.ShowTotal = true;
            tbl.Columns[0].TotalsRowFunction = RowFunctions.Sum;
            tbl.Columns[1].TotalsRowFunction = RowFunctions.Count;
            tbl.Columns[2].TotalsRowFunction = RowFunctions.Average;
            tbl.Columns[3].TotalsRowFunction = RowFunctions.CountNums;
            tbl.DeleteRow(99,1);
            Assert.AreEqual("B1:E100", tbl.Address.Address);
            Assert.AreEqual("Shift Me Up", ws.Cells["B101"].Value);
            tbl.DeleteRow(96, 3);
            Assert.AreEqual("B1:E97", tbl.Address.Address);
            Assert.AreEqual("Don't Shift Me", ws.Cells["F5"].Value);
            Assert.AreEqual("Shift Me Up", ws.Cells["B98"].Value);
        }
        [TestMethod]
        public void TableDeleteRowInside()
        {
            //Setup
            var ws = _pck.Workbook.Worksheets.Add("TableDeleteColumnInside");
            LoadTestdata(ws, 100);
            ws.Cells["A102"].Value = "Shift Me Up";
            var tbl = ws.Tables.Add(ws.Cells["A1:D100"], "TableDeleteColumnInside");
            tbl.DeleteRow(50, 1);
            Assert.AreEqual("A1:D99", tbl.Address.Address);
            Assert.AreEqual("Shift Me Up", ws.Cells["A101"].Value);
            tbl.DeleteRow(75, 3);
            Assert.AreEqual("A1:D96", tbl.Address.Address);
            Assert.AreEqual("Shift Me Up", ws.Cells["A98"].Value);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TableDeleteRowPositionNegative()
        {
            //Setup
            using (var p = new ExcelPackage())
            {
                var ws = p.Workbook.Worksheets.Add("Table1");
                var tbl = ws.Tables.Add(ws.Cells["A1:D100"], "Table1");
                tbl.DeleteRow(-1);
            }
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TableDeleteRowRowsNegative()
        {
            //Setup
            using (var p = new ExcelPackage())
            {
                var ws = p.Workbook.Worksheets.Add("Table1");
                var tbl = ws.Tables.Add(ws.Cells["A1:D100"], "Table1");
                tbl.DeleteRow(0, -1);
            }
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TableDeleteOverTableLimit()
        {
            //Setup
            using (var p = new ExcelPackage())
            {
                var ws = p.Workbook.Worksheets.Add("Table1");
                var tbl = ws.Tables.Add(ws.Cells["A1:D100"], "Table1");
                tbl.DeleteRow(99,1);
            }
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TableDelete5OverTableLimit()
        {
            //Setup
            using (var p = new ExcelPackage())
            {
                var ws = p.Workbook.Worksheets.Add("Table1");
                var tbl = ws.Tables.Add(ws.Cells["A1:D100"], "Table1");
                tbl.DeleteRow(95, 5);
            }
        }
        [TestMethod]
        public void TableDeleteLeaveOneRow()
        {
            //Setup
            var ws = _pck.Workbook.Worksheets.Add("TableLeaveOneRow");
            LoadTestdata(ws, 100);
            var tbl = ws.Tables.Add(ws.Cells["A1:D100"], "TableLeaveOneRow");
            tbl.DeleteRow(0, 98);
            Assert.AreEqual("A1:D2", tbl.Address.Address);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TableDeleteAllRows()
        {
            //Setup
            using (var p = new ExcelPackage())
            {
                var ws = p.Workbook.Worksheets.Add("Table1");
                var tbl = ws.Tables.Add(ws.Cells["A1:D100"], "Table1");
                tbl.DeleteRow(0, 99); 
            }
        }
        #endregion
        #region Insert Column
        [TestMethod]
        public void TableDeleteColumnFirst()
        {
            //Setup
            var ws = _pck.Workbook.Worksheets.Add("TableDeleteColFirst");
            LoadTestdata(ws, 100);

            var tbl = ws.Tables.Add(ws.Cells["A1:D100"], "TableDeleteColFirst");
            ws.Cells["E10"].Value = "Shift Me Left";
            tbl.Columns.Delete(0);

            Assert.AreEqual("A1:C100", tbl.Address.Address);
            Assert.AreEqual("Shift Me Left", ws.Cells["D10"].Value);
            tbl.Columns.Delete(0, 2);
            Assert.AreEqual("A1:A100", tbl.Address.Address);
            Assert.AreEqual("Shift Me Left", ws.Cells["B10"].Value);
            Assert.IsNull(ws.Cells["C10"].Value);
        }
        [TestMethod]
        public void TableDeleteAllColumns()
        {
            //Setup
            var ws = _pck.Workbook.Worksheets.Add("TableAddCol");
            LoadTestdata(ws, 100,2);
            ws.Cells["F99"].Value = "Shift Me Right";
            var tbl = ws.Tables.Add(ws.Cells["B1:E100"], "TableAddColumn");
            tbl.Columns.Delete(0, 4);
            Assert.IsNull(tbl.Address);
            Assert.AreEqual("Shift Me Right", ws.Cells["B99"].Value);
        }
        //[TestMethod]
        //public void TableAddColumnWithTotal()
        //{
        //    //Setup
        //    var ws = _pck.Workbook.Worksheets.Add("TableAddColTotal");
        //    LoadTestdata(ws, 100, 2);
        //    ws.Cells["F100"].Value = "Shift Me Right";
        //    ws.Cells["A50,F102"].Value = "Don't Shift Me";

        //    var tbl = ws.Tables.Add(ws.Cells["B1:E100"], "TableAddTotal");
        //    tbl.ShowTotal = true;
        //    tbl.Columns[0].TotalsRowFunction = RowFunctions.Sum;
        //    tbl.Columns[1].TotalsRowFunction = RowFunctions.Count;
        //    tbl.Columns[2].TotalsRowFunction = RowFunctions.Average;
        //    tbl.Columns[3].TotalsRowFunction = RowFunctions.CountNums;
        //    tbl.Columns.Insert(0, 1);
        //    Assert.AreEqual("B1:F101", tbl.Address.Address);
        //    Assert.AreEqual(RowFunctions.Sum, tbl.Columns[1].TotalsRowFunction);
        //    Assert.AreEqual(RowFunctions.CountNums, tbl.Columns[4].TotalsRowFunction);
        //    Assert.AreEqual("Shift Me Right", ws.Cells["G100"].Value);
        //    tbl.Columns.Add(3);
        //    Assert.AreEqual("B1:I101", tbl.Address.Address);
        //    Assert.AreEqual("Don't Shift Me", ws.Cells["A50"].Value);
        //    Assert.AreEqual("Don't Shift Me", ws.Cells["F102"].Value);
        //}
        //[TestMethod]
        //public void TableInsertColumnInside()
        //{
        //    //Setup
        //    var ws = _pck.Workbook.Worksheets.Add("TableInsertColInside");
        //    LoadTestdata(ws, 100);
        //    ws.Cells["E9999"].Value = "Don't Me Down";
        //    ws.Cells["E19999"].Value = "Don't Me Down";
        //    var tbl = ws.Tables.Add(ws.Cells["A1:D100"], "TableInsertColInside");
        //    tbl.Columns.Insert(4,2);
        //    Assert.AreEqual("A1:F100", tbl.Address.Address);
        //    tbl.Columns.Insert(8, 8);
        //    Assert.AreEqual("A1:N100", tbl.Address.Address);
        //    Assert.AreEqual("Don't Me Down", ws.Cells["E9999"].Value);
        //    Assert.AreEqual("Don't Me Down", ws.Cells["E19999"].Value);
        //}
        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void TableInsertColumnPositionNegative()
        //{
        //    //Setup
        //    using (var p = new ExcelPackage())
        //    {
        //        var ws = p.Workbook.Worksheets.Add("Table1");
        //        var tbl = ws.Tables.Add(ws.Cells["A1:D100"], "Table1");
        //        tbl.Columns.Insert(-1);
        //    }
        //}
        //[TestMethod]
        //public void TableAddColumnToMax()
        //{
        //    //Setup
        //    var ws = _pck.Workbook.Worksheets.Add("TableMaxColumn");
        //    LoadTestdata(ws, 100);
        //    var tbl = ws.Tables.Add(ws.Cells["A1:D100"], "TableMaxColumn");
        //    //Act
        //    tbl.Columns.Add(ExcelPackage.MaxColumns - 4);
        //    //Assert
        //    Assert.AreEqual(ExcelPackage.MaxColumns, tbl.Address._toCol);
        //}
        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void TableAddColumnOverMax()
        //{
        //    using (var p = new ExcelPackage())
        //    {
        //        //Setup
        //        var ws = p.Workbook.Worksheets.Add("TableOverMaxColumn");
        //        LoadTestdata(ws, 100);
        //        var tbl = ws.Tables.Add(ws.Cells["A1:D100"], "TableOverMaxRow");
        //        //Act
        //        tbl.Columns.Add(ExcelPackage.MaxColumns - 3);
        //    }
        //}
        #endregion
    }
}