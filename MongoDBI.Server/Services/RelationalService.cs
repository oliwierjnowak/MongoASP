using System.Data.SqlClient;
using System.Diagnostics;
using Dapper;
namespace MongoDBI.Server.Services
{

    public class RelationalService
    {

        //createAsync(Employee newEmp)
        public async Task<(long, int)> CreateShift(int howoften)
        {
            Random rnd = new Random();
            EmployeeShift employeeShift = new EmployeeShift()
            {
                DOW = 1,
                EmpNr = 1,
                ISOWeek = 1,
                ISOYear = 1,
                ShiftHours = 1,
                ShiftName = "1",
                ShiftNR = 1
            };
            int i = 0;
            Stopwatch sw = Stopwatch.StartNew();
            SqlConnection _connection = new SqlConnection("Server=localhost,1433;User ID=SA;Password=A@123!23sda;Trusted_Connection=False;Encrypt=False;");
            while (i < howoften)
            {
        
            
              
                var dow = "";

                switch (employeeShift.DOW)
                {
                    case 1:
                        dow = "dosh_monday";
                        break;
                    case 2:
                        dow = "dosh_tuesday";
                        break;
                    case 3:
                        dow = "dosh_wednesday";
                        break;
                    case 4:
                        dow = "dosh_thursday";
                        break;
                    case 5:
                        dow = "dosh_friday";
                        break;
                    case 6:
                        dow = "dosh_saturday";
                        break;
                    case 7:
                        dow = "dosh_sunday";
                        break;
                }
                var day = employeeShift.DOW;
                    // insert
                    string insert = @$"
    insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday] ) 
    values (@EmpNr, @ISOWeek, @ISOYear, {(day == 1 ? "@ShiftNR" : "1")},
									    {(day == 2 ? "@ShiftNR" : "1")},
									    {(day == 3 ? "@ShiftNR" : "1")},
									    {(day == 4 ? "@ShiftNR" : "1")},
									    {(day == 5 ? "@ShiftNR" : "1")},
									    {(day == 6 ? "@ShiftNR" : "1")},
									    {(day == 7 ? "@ShiftNR" : "1")});";

                    var x = await _connection.ExecuteAsync(insert, employeeShift);
                i++;
                }
            sw.Stop();
            _connection.Close();
            var time = sw.ElapsedMilliseconds;
            return (time, 1);
            
            

        }

        //Task<Employee> UpdateAsync(int dono, Employee updatedEmployee);
        public async Task<(long, int)> UpdateShift()
        {

            EmployeeShift employeeShift = new EmployeeShift()
            {
                DOW = 2,
                EmpNr = 1,
                ISOWeek = 2,
                ISOYear = 2,
                ShiftHours = 2,
                ShiftName = "2",
                ShiftNR = 1
            };
            SqlConnection _connection = new SqlConnection("Server=localhost,1433;User ID=SA;Password=A@123!23sda;Trusted_Connection=False;Encrypt=False;");
            var dow = "";
            Stopwatch sw = Stopwatch.StartNew();
            switch (employeeShift.DOW)
            {
                case 1:
                    dow = "dosh_monday";
                    break;
                case 2:
                    dow = "dosh_tuesday";
                    break;
                case 3:
                    dow = "dosh_wednesday";
                    break;
                case 4:
                    dow = "dosh_thursday";
                    break;
                case 5:
                    dow = "dosh_friday";
                    break;
                case 6:
                    dow = "dosh_saturday";
                    break;
                case 7:
                    dow = "dosh_sunday";
                    break;
            }
            var day = employeeShift.DOW;
            // insert
            string insert = @$"update [dbo].[csti_do_shift] 
									set  dosh_week_number = @ISOWeek, dosh_year = @ISOYear, [{dow}] = @ShiftNR
									where
									dosh_do_no = @EmpNr and dosh_week_number = @ISOWeek and dosh_year = @ISOYear";

            var x = await _connection.ExecuteAsync(insert, employeeShift);
            sw.Stop();
            _connection.Close();
            var time = sw.ElapsedMilliseconds;
            return (time, x);


        }

        // Task<List<Employee>> GetAsync(); (ohne Filter
        public async Task<(long,List<EmployeeShift>)> Find1()
        {

            SqlConnection _connection = new SqlConnection("Server=localhost,1433;User ID=SA;Password=A@123!23sda;Trusted_Connection=False;Encrypt=False;");

            string shiftOverview = @$"
								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear'
								from [dbo].[csti_do_shift] ";
            _connection.Open();
            string selectQuery = shiftOverview;
            Stopwatch sw = Stopwatch.StartNew();
            IEnumerable<EmployeeShift> data = await _connection.QueryAsync<EmployeeShift>(selectQuery);
            sw.Stop();
            _connection.Close();
            var time = sw.ElapsedMilliseconds;
            return (time, (List<EmployeeShift>)data);
        }

        //     Task<Employee> GetSingleAsync(int dono);  mit Filter
        public async Task<(long,List<EmployeeShift>)> Find2(int superior, int emp)
        {
            SqlConnection _connection = new SqlConnection("Server=localhost,1433;User ID=SA;Password=A@123!23sda;Trusted_Connection=False;Encrypt=False;");
            string shiftOverview = @$"
								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 1) as 'DOW',dosh_monday as 'ShiftNR'  , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_monday = EmpShift.ds_no 
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 2) as 'DOW',dosh_wednesday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_wednesday = EmpShift.ds_no 
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 3) as 'DOW',dosh_wednesday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_wednesday = EmpShift.ds_no 
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 4) as 'DOW',dosh_thursday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_thursday = EmpShift.ds_no 
								union all


								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 5) as 'DOW',dosh_friday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours' 	
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_friday = EmpShift.ds_no 
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 6) as 'DOW',dosh_saturday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_saturday = EmpShift.ds_no	
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 7) as 'DOW',dosh_sunday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_sunday = EmpShift.ds_no ";
            _connection.Open();
            string selectQuery = shiftOverview;
            Stopwatch sw = Stopwatch.StartNew();
            IEnumerable<EmployeeShift> data = await _connection.QueryAsync<EmployeeShift>(selectQuery, new
            {
                emp = emp,
                superior = superior
            });
            sw.Stop();
            _connection.Close();
            var time = sw.ElapsedMilliseconds;
            return (time, (List<EmployeeShift>)data);
        }

        //mit Filter und Projektion
        public async Task<(long, List<DailyScheduleDTO>)> Find3(int Shift)
        {
            SqlConnection _connection = new SqlConnection("Server=localhost,1433;User ID=SA;Password=A@123!23sda;Trusted_Connection=False;Encrypt=False;");
            string shiftQuery = @$"select ds_no AS 'Nr',ds_name AS 'Name' ,ds_hours AS 'Hours',ds_color AS 'Color' from [dbo].[csti_daily_schedule] where ds_no = {Shift}";
            _connection.Open();
            string selectQuery = shiftQuery;
           
            Stopwatch sw = Stopwatch.StartNew();

            IEnumerable<DailyScheduleDTO> data = await _connection.QueryAsync<DailyScheduleDTO>(selectQuery);
            _connection.Close();

            sw.Stop();
            _connection.Close();
            var time = sw.ElapsedMilliseconds;
            
            return (time,(List<DailyScheduleDTO>)data);
        }

        //  mit Filter, Projektion und Sortierung)
        public async Task<(long,List<DailyScheduleDTO>)> Find4()
        {
            SqlConnection _connection = new SqlConnection("Server=localhost,1433;User ID=SA;Password=A@123!23sda;Trusted_Connection=False;Encrypt=False;");
            _connection.Open();
            string selectQuery = "select ds_no AS 'Nr',ds_name AS 'Name' ,ds_hours AS 'Hours', ds_color AS 'Color'   from [dbo].[csti_daily_schedule] where ds_hours >0 order by ds_hours ";

            Stopwatch sw = Stopwatch.StartNew();

            IEnumerable<DailyScheduleDTO> data = await _connection.QueryAsync<DailyScheduleDTO>(selectQuery);

            sw.Stop();
            _connection.Close();
            var time = sw.ElapsedMilliseconds;

            return (time,(List<DailyScheduleDTO>)data);
        }

        public async Task<(long,int)> Delete()
        {
            SqlConnection _connection = new SqlConnection("Server=localhost,1433;User ID=SA;Password=A@123!23sda;Trusted_Connection=False;Encrypt=False;");
           
            string insert = @$" insert into[dbo].[csti_daily_schedule] ([ds_name], [ds_hours], [ds_color]) values('aaaa', 123, '#11111');"; 

            var x = await _connection.ExecuteAsync(insert);

            SqlConnection _connection2 = new SqlConnection("Server=localhost,1433;User ID=SA;Password=A@123!23sda;Trusted_Connection=False;Encrypt=False;");
            string delete = @$"delete from [dbo].[csti_daily_schedule] Output deleted.ds_no where ds_hours = 123;";


            Stopwatch sw = Stopwatch.StartNew();
            var x2 = await _connection.QueryFirstAsync<int>(delete);

            sw.Stop();
            _connection.Close();
            var time = sw.ElapsedMilliseconds;

            return (time,x2);
        }
    }




    public record EmployeeShift
    {
        public int EmpNr { get; set; }
        public int ISOWeek { get; set; }
        public int ISOYear { get; set; }
        public int DOW { get; set; }
        public int ShiftNR { get; set; }
        public string ShiftName { get; set; }
        public int ShiftHours { get; set; }

    }

    public record DailyScheduleDTO
    {
        public int Nr { get; set; }
        public string Name { get; set; }
        public int Hours { get; set; }
        public string Color { get; set; }
    }
}
