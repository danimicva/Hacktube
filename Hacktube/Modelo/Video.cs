using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hacktube
{
    class Video
    {
        public enum ESTADO
        {
            INICIAL,
            BUSCANDO_URL,
            CON_URL,
            ERROR_URL,
            DESCARGANDO,
            DESCARGADO,
            ERROR_DESCARGA
        }

        public string UrlYoutube { get; }
        public string Nombre { get; private set; }
        public string UrlDescarga { get; private set; } 
        public ESTADO Estado { get; private set; }
        
        public Video(string urlYoutube, string nombre = "", string url = "" )
        {
            UrlYoutube = urlYoutube;
            Nombre = nombre;
        }

        public void obtenerInfo()
        {
            if (Estado != ESTADO.ERROR_URL && Estado != ESTADO.INICIAL)
                throw new ObtenerUrlException("El estado del vídeo no es correcto: " + Estado);

            if (string.IsNullOrEmpty(UrlYoutube))
            {
                Estado = ESTADO.ERROR_URL;
                throw new ObtenerUrlException("La url de youtube está vacía");
            }

            try
            {
                Estado = ESTADO.BUSCANDO_URL;
                InfoVideo InfoVideo = YoutubeUtils.obtenerUrlDescargaYNombreFullUrl(UrlYoutube);
                UrlDescarga = InfoVideo.Url;
                Nombre = InfoVideo.Nombre;
                Estado = ESTADO.CON_URL;
            }
            catch (Exception ex)
            {
                Estado = ESTADO.ERROR_URL;
                throw new ObtenerUrlException(ex.Message);
            }
        }

        public void descargar(string ruta)
        {
            if (Estado != ESTADO.ERROR_DESCARGA && Estado != ESTADO.CON_URL)
                throw new ObtenerUrlException("El estado del vídeo no es correcto: " + Estado);

            if (String.IsNullOrEmpty(UrlDescarga))
            {
                Estado = ESTADO.ERROR_URL;
                throw new DescargaException("La url de descarga está vacía");
            }
            
            try
            {
                Estado = ESTADO.DESCARGANDO;
                YoutubeUtils.descargarVideo(UrlDescarga, ruta);
                Estado = ESTADO.DESCARGADO;
            }
            catch (Exception ex)
            {
                Estado = ESTADO.ERROR_DESCARGA;
                throw new DescargaException(ex.Message);
            }
        }
    }
}
