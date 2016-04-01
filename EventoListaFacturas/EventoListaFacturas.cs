using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace LaboratorioFacturas.EventoListaFacturas
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class EventoListaFacturas : SPItemEventReceiver
    {
        //totalizar los resultados del cambio en una propiedad
        private void UpdatePropertyBag(SPWeb web, double cambio)
        {
            string KeyName = "TotalFacturas";

            double actual = 0;

            if (web.Properties[KeyName]!=null)
            {
                actual = double.Parse(web.Properties[KeyName]);
            }
            else
            {
                web.Properties.Add(KeyName,"");
            }

            actual += cambio;
            web.Properties[KeyName] = actual.ToString();
            web.Properties.Update();
        }

        //capturar la adicion de un item y la actualizacion del total
        public override void ItemAdding(SPItemEventProperties properties)
        {
            double valor;
            double.TryParse(properties.AfterProperties["Importe"].ToString(), out valor);
            UpdatePropertyBag(properties.Web, valor);
        }
        
        // edicion de un item y acutalizacion del total
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            double valorPrevio, nuevoValor;
            double.TryParse(properties.ListItem["Importe"].ToString(), out valorPrevio);
            double.TryParse(properties.AfterProperties["Importe"].ToString(), out nuevoValor);
            double change = nuevoValor-valorPrevio;
            UpdatePropertyBag(properties.Web, change);

        }

        //eliminacion de un item y la actualizacion del total
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            double valor;
            double.TryParse(properties.ListItem["Importe"].ToString(), out valor);
            UpdatePropertyBag(properties.Web,-valor);
        }




    }
}