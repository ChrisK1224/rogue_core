//*****************************************************************************************
//*                                                                                       *
//* This is an auto-generated file by Microsoft ML.NET CLI (Command-Line Interface) tool. *
//*                                                                                       *
//*****************************************************************************************

using Microsoft.ML.Data;
using rogue_core.rogueCore.hqlSyntaxV4;

namespace rogueCore.hqlSyntaxV4.location.column.command.models
{
    public class CryptoLagInput
    {
        [ColumnName("close"), LoadColumn(0)]
        public float Close { get; set; }


        [ColumnName("DAYOFWEEK"), LoadColumn(1)]
        public float DAYOFWEEK { get; set; }


        [ColumnName("LAGGER"), LoadColumn(2)]
        public float LAGGER { get; set; }


        [ColumnName("LAGGER2"), LoadColumn(3)]
        public float LAGGER2 { get; set; }


        [ColumnName("LAGGER3"), LoadColumn(4)]
        public float LAGGER3 { get; set; }


        [ColumnName("LAGGER4"), LoadColumn(5)]
        public float LAGGER4 { get; set; }


        [ColumnName("LAGGER5"), LoadColumn(6)]
        public float LAGGER5 { get; set; }


        [ColumnName("LAGGER6"), LoadColumn(7)]
        public float LAGGER6 { get; set; }


        [ColumnName("LAGGER7"), LoadColumn(8)]
        public float LAGGER7 { get; set; }

        public CryptoLagInput(){}
    }
}
