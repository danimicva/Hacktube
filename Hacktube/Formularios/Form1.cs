
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hacktube
{
    public partial class Form1 : Form
    {
        #region funciones portapapeles

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWnd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(
            IntPtr hWndRemove,  // handle to window to remove
            IntPtr hWndNewNext  // handle to next window
            );

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        IntPtr _ClipboardViewerNext;


        private void registrarEscuchaPortapapeles()
        {
            _ClipboardViewerNext = SetClipboardViewer(this.Handle);
        }

        private void desregistrarEscuchaPortapapeles()
        {
            ChangeClipboardChain(this.Handle, _ClipboardViewerNext);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                //
                // The WM_DRAWCLIPBOARD message is sent to the first window 
                // in the clipboard viewer chain when the content of the 
                // clipboard changes. This enables a clipboard viewer 
                // window to display the new content of the clipboard. 
                //
                case 0x0308: // WM_DRAWCLIPBOARD

                    //Debug.WriteLine("WindowProc DRAWCLIPBOARD: " + m.Msg, "WndProc");

                    string cb = GetClipboardData();
                    if (cb.Contains("youtube.com/watch?v=") || cb.Contains("youtu.be/"))
                        nuevoEnlaceCopiado(cb);
                    //
                    // Each window that receives the WM_DRAWCLIPBOARD message 
                    // must call the SendMessage function to pass the message 
                    // on to the next window in the clipboard viewer chain.
                    //
                    SendMessage(_ClipboardViewerNext, m.Msg, m.WParam, m.LParam);
                    break;

                default:
                    //
                    // Let the form process the messages that we are
                    // not interested in
                    //
                    base.WndProc(ref m);
                    break;
            }
        }

        private string GetClipboardData()
        {
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                string clipboardText = Clipboard.GetText(TextDataFormat.Text);
                return clipboardText;
            }

            return "";
        }
        #endregion

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            registrarEscuchaPortapapeles();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            desregistrarEscuchaPortapapeles();
        }

        private void nuevoEnlaceCopiado(string enlace)
        {
            annadirVideo(enlace);
        }
        
        private void btnAnnadir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbAnnadir.Text))
            {
                MessageBox.Show("La ruta no puede estar vacía");
                return;
            }

            annadirVideo(tbAnnadir.Text);
        }

        private void annadirVideo(string url)
        {
            Video video = new Video(url);

            dgvDescargas.Rows.Add(generarRow(video));
        }

        private DataGridViewRow generarRow(Video video)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.Cells.Add(new DataGridViewTextBoxCell());
            row.Cells[0].Value = video.UrlYoutube;
            row.Cells.Add(new DataGridViewTextBoxCell());
            row.Cells[1].Value = video.Estado;
            row.Cells.Add(new DataGridViewButtonCell());
            row.Cells[2].Value = "Buscar Url";
            row.Cells.Add(new DataGridViewButtonCell());
            row.Cells[3].Value = "Eliminar";
            row.Tag = video;

            return row;
        }

        private void dgvDescargas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDescargas.Rows[e.RowIndex];
                Video video = row.Tag as Video;
                if (video == null)
                {
                    MessageBox.Show("Error obteniendo el vídeo del Tag de la fila");
                    return;
                }

                if (e.ColumnIndex == 2)
                    ejecutarAccionVideoConTask(video, row);
                else if (e.ColumnIndex == 3)
                    eliminarVideo(row);
            }
        }

        private void eliminarVideo(DataGridViewRow row)
        {
            dgvDescargas.Rows.Remove(row);
        }
        
        private void ejecutarAccionVideoConTask(Video video, DataGridViewRow row)
        {
            var uiContext = TaskScheduler.FromCurrentSynchronizationContext();

            Task.Factory.StartNew(() =>
                {
                    ejecutarAccionVideo(video, row, uiContext);
                });
        }

        private void modificarUiDesdeTask(Action action, TaskScheduler uiContext)
        {
            Task.Factory.StartNew(
                action, 
                CancellationToken.None, 
                TaskCreationOptions.None,
                uiContext);
        }

        private void ejecutarAccionVideo(Video video, DataGridViewRow row, TaskScheduler uiContext = null)
        {
            string nuevaAccion;
            nuevaAccion = row.Cells[2].Value.ToString();
            try
            {
                switch (video.Estado)
                {
                    case Video.ESTADO.ERROR_URL:
                    case Video.ESTADO.INICIAL:
                        if (uiContext != null)
                        {
                            modificarUiDesdeTask(() =>
                            {
                                row.Cells[1].Value = "Obteniendo URL";
                            }, uiContext);
                        }
                        video.obtenerInfo();
                        row.Cells[0].Value = video.Nombre;
                        nuevaAccion = "Descargar";
                        break;
                    case Video.ESTADO.CON_URL:
                    case Video.ESTADO.ERROR_DESCARGA:
                        if (uiContext != null)
                        {
                            modificarUiDesdeTask(() =>
                            {
                                row.Cells[1].Value = "Descargando";
                            }, uiContext);
                        }
                        string nombre = row.Cells[0].Value.ToString();
                        //Reemplazamos todos los caracteres que windows no quiere

                        string limpio = nombre.Replace("\\", "").Replace("\"","").Replace("/","").Replace("*", "").Replace("?","")
                            .Replace("<","").Replace(">","").Replace("|","").Replace(":", "");

                        video.descargar("C:\\" + limpio + ".mp4");
                        nuevaAccion = "Finalizado";
                        break;
                    default:
                        MessageBox.Show("No hay acción asociada al estado: " + video.Estado);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ejecutando acción: " + ex.Message);
            }
            finally
            {
                if (uiContext != null)
                {
                    modificarUiDesdeTask(() =>
                    {
                        row.Cells[1].Value = video.Estado;
                        row.Cells[2].Value = nuevaAccion;
                    }, uiContext);
                }
                else
                {
                    row.Cells[1].Value = video.Estado;
                    row.Cells[2].Value = nuevaAccion;
                }
            }
        }

        private void tbAnnadir_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                btnAnnadir.PerformClick();
        }
    }
}
