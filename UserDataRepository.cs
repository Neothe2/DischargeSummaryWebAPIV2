using Oracle.ManagedDataAccess.Client;

namespace DischargeSummaryWebAPIV2
{
    internal class UserDataRepository
    {
        private readonly string connectionString = "User Id=Mediware;Password=medi;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=192.168.10.249)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=dsoft)));Connection Timeout=200;";

        public PatientData getDataOfPatient(string IpNo)
        {
            if (!int.TryParse(IpNo, out int ipNumberToSearch))
            {
                throw new ArgumentException("Invalid input. Please enter a valid IP number.");
            }

            PatientData patientData = new PatientData();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                //string selectQuery = @"SELECT i.IP_NO,i.PT_NO,i.IPD_DATE,i.IPD_DISC,i.PTC_PTNAME,i.PTC_SEX,i.PTD_DOB,r.IRC_REMARKS FROM IPADMISS i JOIN IPPATIENTROUNDS@link_Clinical r ON i.IP_NO = r.IP_NO WHERE i.IP_NO = :ipNumber ORDER BY r.VSD_DATE";
                string selectQuery = @"SELECT i.IP_NO,i.PT_NO,i.IPD_DATE,i.IPD_DISC,i.PTC_PTNAME,i.PTC_SEX,i.PTD_DOB,i.PTC_LOADD1,i.PTC_LOADD2,i.PTC_LOADD3,i.PTC_LOADD4,i.PTC_LOPIN,i.PTC_LOPHONE,r.IRC_REMARKS FROM IPADMISS i JOIN IPPATIENTROUNDS@link_Clinical r ON i.IP_NO = r.IP_NO  WHERE i.IP_NO = :ipNumber ORDER BY r.VSD_DATE";
                //string selectQuery = @"SELECT IP_NO,PT_NO,VSD_DATE,ENT_DATE,IRC_REMARKS FROM IPPATIENTROUNDS WHERE IP_NO = :ipNumber ORDER BY VSD_DATE";

                using (OracleCommand cmd = new OracleCommand(selectQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("ipNumber", ipNumberToSearch));

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        bool isFirstRow = true;
                        while (reader.Read())
                        {
                            if (isFirstRow)
                            {
                                patientData.IpNumber = reader["IP_NO"].ToString();
                                patientData.PatientNumber = reader["PT_NO"].ToString();
                                patientData.PatientName = reader["PTC_PTNAME"].ToString();
                                patientData.Gender = reader["PTC_SEX"].ToString();
                                patientData.DateOfBirth = reader["PTD_DOB"].ToString();
                                patientData.AdmissionDate = reader["IPD_DATE"].ToString();
                                patientData.DischargeDate = reader["IPD_DISC"].ToString();
                                patientData.AddLine1 = reader["PTC_LOADD1"].ToString();
                                patientData.AddLine2 = reader["PTC_LOADD2"].ToString();
                                patientData.AddLine3 = reader["PTC_LOADD3"].ToString();
                                patientData.AddLine4 = reader["PTC_LOADD4"].ToString();
                                patientData.PinCode = reader["PTC_LOPIN"].ToString();
                                patientData.PhoneNumber = reader["PTC_LOPHONE"].ToString();
                                isFirstRow = false;
                            }

                            patientData.DailyRemarks.Add(new DailyRemark
                            {
                                Remarks = reader["IRC_REMARKS"].ToString()
                            });
                        }
                    }
                }
            }

            if (patientData.IpNumber == null)
            {
                throw new KeyNotFoundException("No patient found with that IP number.");
            }

            return patientData;
        }

        public List<IpNumber> GetDataOfPatient(string ptNo)
        {
            if (!int.TryParse(ptNo, out int ptNumberToSearch))
            {
                throw new ArgumentException("Invalid input. Please enter a valid IP number.");
            }

            //PatientData patientData = new PatientData();
            List<IpNumber> ipNumbers = new List<IpNumber>();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string selectQuery = @"SELECT DISTINCT i.PT_NO,i.IP_NO FROM IPADMISS i JOIN IPPATIENTROUNDS@link_Clinical r ON i.IP_NO = r.IP_NO WHERE i.PT_NO = :ptNumber ORDER BY i.PT_NO";

                using (OracleCommand cmd = new OracleCommand(selectQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("ptNumber", ptNumberToSearch));

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        bool isFirstRow = true;
                        while (reader.Read())
                        {


                            ipNumbers.Add(new IpNumber
                            {
                                Ipnum = reader["IP_NO"].ToString()
                            });
                        }
                    }
                }
            }

            if (ipNumbers == null)
            {
                throw new KeyNotFoundException("No patient found with that IP number.");
            }

            return ipNumbers;
        }


    }

    public class PatientData
    {
        public string IpNumber { get; set; }
        public string PatientNumber { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string AddLine1 { get; set; }
        public string AddLine2 { get; set; }
        public string AddLine3 { get; set; }
        public string AddLine4 { get; set; }
        public string PinCode { get; set; }
        public string PhoneNumber { get; set; }
        public string AdmissionDate { get; set; }
        public string DischargeDate { get; set; }
        public List<DailyRemark> DailyRemarks { get; set; } = new List<DailyRemark>();
    }

    public class IpNumber
    {
        public string Ipnum { get; set; }
    }

    public class DailyRemark
    {
        public string Remarks { get; set; }
    }
}
