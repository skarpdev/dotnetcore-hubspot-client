using System;
using System.Collections.Generic;
using System.Text;

namespace Skarp.HubSpotClient.Core
{
    public enum HubSpotAssociationDefinitions
    {
        CompanyToCompany = 1,

        CompanyToContact,

        DealToContact,

        ContactToDeal,

        DealToCompany,

        CompanyToDeal,

        CompanyToEngagement,

        EngagementToCompany,

        ContactToEngagement,

        EngagementToContact,

        DealToEngagement,

        EngagementToDeal,


        ParentCompanyToChildCompany,

        ChildCompanyToParentCompany,

        ContactToTicket,

        TicketToContact,
        
        TicketToEngagement,

        EngagementToTicket,

        DealToLineItem,

        LineItemToDeal,

        CompanyToTicket = 25,

        TicketToCompany,

        DealToTicket,
        
        TicketToDeal
    }
}
