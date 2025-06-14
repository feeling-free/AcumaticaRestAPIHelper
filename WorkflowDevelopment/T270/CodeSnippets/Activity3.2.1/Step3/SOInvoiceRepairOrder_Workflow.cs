﻿using PX.Data.WorkflowAPI;
using PX.Data;
using PX.Objects.SO;
using PX.Objects.AR;
using PX.Common;
using PX.Data.BQL;

namespace PhoneRepairShop
{
    public class SOInvoiceRepairOrder_Workflow : 
        PXGraphExtension<SOInvoiceEntry_Workflow, SOInvoiceEntry>
    {
        ////////// The added code
        public const string ApproveDiscount = "Approve Discount";
        ////////// The end of added code

        public static class ActionCategories
        {
            public const string RepairCategoryID = "Repair Work Orders Category";

            [PXLocalizable]
            public static class DisplayNames
            {
                public const string RepairOrders = "Repair Work Orders";
            }
        }

        public sealed override void Configure(PXScreenConfiguration config)
        {
            Configure(config.GetScreenConfigurationContext<SOInvoiceEntry,
                                                           ARInvoice>());
        }

        protected static void Configure(WorkflowContext<SOInvoiceEntry,
                                                        ARInvoice> context)
        {
            var repairCategory = context.Categories.CreateNew(
                ActionCategories.RepairCategoryID,
                category => category.DisplayName(
                ActionCategories.DisplayNames.RepairOrders));

            #region Action Definitions
            var viewOrder = context.ActionDefinitions
              .CreateExisting<SOInvoiceEntry_Extension>(graph => graph.ViewOrder,
                action => action.WithCategory(repairCategory));
            ////////// The added code
            var approveDiscount = context.ActionDefinitions
                .CreateNew(ApproveDiscount, action => action
                .DisplayName("Approve Discount"));
            ////////// The end of added code
            #endregion

            context.UpdateScreenConfigurationFor(screen => screen
                .UpdateDefaultFlow(flow =>
                {
                    return flow
                        .WithFlowStates(flowStates =>
                        {
                            flowStates.Update<ARDocStatus.open>(flowState =>
                            {
                                return flowState.WithActions(actions =>
                                    actions.Add(viewOrder));
                            });
                        });
                })
                .WithCategories(categories =>
                {
                    categories.Add(repairCategory);
                })
                .WithActions(actions =>
                {
                    actions.Add(viewOrder);
                    ////////// The added code
                    actions.Add(approveDiscount);
                    ////////// The end of added code
                })
                .WithFieldStates(fs =>
                {
                    fs.Add<ARInvoice.status>(state =>
                        state.SetComboValue(ARDocStatus_Postponed
                          .Postponed, "Postponed"));
                })
            );
        }
    }

    public class ARDocStatus_Postponed : ARDocStatus
    {
        public const string Postponed = "O";
        public class postponed : BqlType<IBqlString, string>.Constant<postponed>
        {
            public postponed()
                : base("O")
            {
            }
        }
    }
}
