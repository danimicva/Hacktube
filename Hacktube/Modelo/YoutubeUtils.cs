using NHtmlUnit;
using NHtmlUnit.Html;
using System;

namespace Hacktube
{
    class InfoVideo
    {
        public string Nombre;
        public string Url;
        public InfoVideo(string nombre, string url)
        {
            Nombre = nombre;
            Url = url;
        }
    }
    class YoutubeUtils
    {
        
        public static Video obtenerVideoPorUrl(string fullUrl, int reintentos = 10)
        {
            WebClient cliente;
            string url, nombre;
            int reintentosActuales = 0;
            HtmlTextInput input;
            HtmlButton submit;

            cliente = obtenerCliente();

            HtmlPage paginaDescarga = (HtmlPage)cliente.GetPage(Constantes.URL_INICIAL);
            try
            {
                input = (HtmlTextInput)paginaDescarga.GetElementById("yt");
            }
            catch (Exception)
            {
                cliente.Close();
                throw new HacktubeException("No se ha encontrado el input \"yt\"");
            }

            try
            {
                submit = (HtmlButton)paginaDescarga.GetElementById("submit");
            }
            catch (Exception)
            {
                cliente.Close();
                throw new HacktubeException("No se ha encontrado boton \"submit\"");
            }

            input.SetValueAttribute(fullUrl);
            HtmlPage temp = (HtmlPage)submit.Click();

            while (!videoYaCargado(temp) && reintentosActuales++ < reintentos)
                cliente.WaitForBackgroundJavaScript(1000);

            if (!videoYaCargado(temp))
            {
                cliente.Close();
                throw new HacktubeException("No se ha cargado el vídeo en el tiempo estipulado");
            }

            try
            {
                HtmlElement div = temp.GetElementById("ytform3");
                HtmlElement elem = (HtmlElement)div.GetElementsByTagName("a")[0];

                url = elem.GetAttribute("href");
                nombre = elem.GetAttribute("href");



                cliente.Close();
                return new Video(url, nombre);
            }
            catch (Exception ex)
            {
                cliente.Close();
                throw new HacktubeException("No se ha podido encontrar el link una vez cargado el vídeo: " + ex.Message);
            }
        }

        public static InfoVideo obtenerUrlDescargaYNombreFullUrl(string fullUrl, int reintentos = 10)
        {
            WebClient cliente;
            string url, nombre;
            int reintentosActuales = 0;
            HtmlTextInput input;
            HtmlButton submit;

            cliente = obtenerCliente();

            HtmlPage paginaDescarga = (HtmlPage) cliente.GetPage(Constantes.URL_INICIAL);
            try
            {
                input = (HtmlTextInput)paginaDescarga.GetElementById("yt");
            }
            catch (Exception)
            {
                cliente.Close();
                throw new HacktubeException("No se ha encontrado el input \"yt\"");
            }

            try
            {
                submit = (HtmlButton)paginaDescarga.GetElementById("submit");
            }
            catch (Exception)
            {
                cliente.Close();
                throw new HacktubeException("No se ha encontrado boton \"submit\"");
            }

            input.SetValueAttribute(fullUrl);
            HtmlPage temp = (HtmlPage)submit.Click();

            while (!videoYaCargado(temp) && reintentosActuales++ < reintentos)
                cliente.WaitForBackgroundJavaScript(1000);

            if(!videoYaCargado(temp)) { 
                cliente.Close();
                throw new HacktubeException("No se ha cargado el vídeo en el tiempo estipulado");
            }

            try
            {
                HtmlElement div = temp.GetElementById("ytform3");
                HtmlElement elem = (HtmlElement)div.GetElementsByTagName("a")[0];

                url = elem.GetAttribute("href");
                nombre = temp.GetElementById("ytitle").TextContent;
                cliente.Close();
                return new InfoVideo(nombre, url);
            }
            catch (Exception ex)
            {
                cliente.Close();
                throw new HacktubeException("No se ha podido encontrar el link una vez cargado el vídeo: " + ex.Message);
            }
        }

        public static InfoVideo obtenerUrlDescargaPorId(string idVideo, int reintentos = 10)
        {
            return obtenerUrlDescargaYNombreFullUrl(Constantes.YOUTUBE_BASE + idVideo, reintentos);
        }

        private static WebClient obtenerCliente()
        {
            WebClient cliente = new WebClient(BrowserVersion.CHROME);

            cliente.Options.JavaScriptEnabled = true;
            cliente.Options.ThrowExceptionOnScriptError = false;
            return cliente;
        }

        private static bool videoYaCargado(HtmlPage pagina)
        {
            try
            {
                HtmlElement elem = pagina.GetElementById("ytform2");
                HtmlElement elem2 = elem.GetElementByClassName("alert");

                if (elem2 == null)
                    return true;

                return false; 

            }
            catch (Exception ex)
            {
                throw new HacktubeException("Error consultando si el vídeo ya está cargado: " + ex.ToString());
            }
        }

        public static void descargarVideo(String url, string ruta)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            client.DownloadFile(url, ruta);
        }

    }
}
    