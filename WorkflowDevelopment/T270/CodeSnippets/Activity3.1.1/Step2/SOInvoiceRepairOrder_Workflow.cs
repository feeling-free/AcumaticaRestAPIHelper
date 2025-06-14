﻿using PX.Data.WorkflowAPI;
using PX.Data;
using PX.Objects.SO;
using PX.Objects.AR;

namespace PhoneRepairShop
{
public class SOInvoiceRepairOrder_Workflow : 
        PXGraphExtension<SOInvoiceEntry_Workflow, SOInvoiceEntry>
    {
        public sealed override void Configure(PXScreenConfiguration config)
        {
            Configure(config.GetScreenConfigurationContext<SOInvoiceEntry,
                                                           ARInvoice>());
        }

        protected static void Configure(WorkflowContext<SOInvoiceEntry,
                                                        ARInvoice> context)
        {
        }
    }
}
