﻿using System;
using Kengic.Was.Domain.Entity.WorkTask.WorkTasks;

namespace Kengic.Was.Domain.Entity.Complement
{
    public class ComplementSourceWorkTask : SourceWorkTask<string>
    {
        public string ScannerBarcode { get; set; }
        public string LogicBarcode { get; set; }
        public string AtricleBarcode { get; set; }

        public string ComplementBarcode { get; set; }

        public string ComplementDestination { get; set; }

        public string AtricleLength { get; set; }
        public string AtricleWidth { get; set; }
        public string AtricleHeight { get; set; }
        public string AtricleProfile { get; set; }
        public string AtricleWeight { get; set; }
        public string AtricleVolume { get; set; }
        public string NodeId { get; set; }
        public string PhysicalSortter { get; set; }
        public string LogicalSortter { get; set; }
        public string Induct { get; set; }
        public DateTime? InductTime { get; set; }
        public DateTime? ScannerTime { get; set; }
        public string Scanner { get; set; }
        public string CarrierId { get; set; }
        public string FinalCarrierId { get; set; }
        public string TrackingId { get; set; }
        public string RequestShuteNum { get; set; }
        public string RequestShuteCode { get; set; }
        public string RequestShuteAddr { get; set; }
        public string RequestShuteAddrX { get; set; }
        public string RequestShuteAddrY { get; set; }
        public string RequestShuteAddrZ { get; set; }
        public string CurrentShuteCode { get; set; }
        public string CurrentShuteAddr { get; set; }
        public string CurrentShuteAddrX { get; set; }
        public string CurrentShuteAddrY { get; set; }
        public string CurrentShuteAddrZ { get; set; }
    }
}