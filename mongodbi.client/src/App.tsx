import React, { useEffect, useState } from "react";
import "./App.css";

interface Workday {
  workdays_id: number;
}

interface Shift {
  isOweek: number;
  year: number;
  monday: Workday;
  tuesday: Workday;
  wednesday: Workday;
  thursday: Workday;
  friday: Workday;
  saturday: Workday;
  sunday: Workday;
}

interface Employee {
  id: string;
  do_no: number;
  do_name: string;
  shifts: Shift[];
}

interface ShiftType {
  _id: number;
  shiftname: string;
  shifthours: number;
}

const shiftData: ShiftType[] = [
  {
    _id: 1,
    shiftname: "A",
    shifthours: 8,
  },
  {
    _id: 0,
    shiftname: "0",
    shifthours: 0,
  },
  {
    _id: 3,
    shiftname: "B",
    shifthours: 4,
  },
  {
    _id: 4,
    shiftname: "Christ",
    shifthours: 24,
  },
];

const shiftContent = (
  <table className="table table-striped">
    <thead>
      <tr>
        <th>ID</th>
        <th>Shift Name</th>
        <th>Shift Hours</th>
      </tr>
    </thead>
    <tbody>
      {shiftData.map((shift) => (
        <tr key={shift._id}>
          <td>{shift._id}</td>
          <td>{shift.shiftname}</td>
          <td>{shift.shifthours}</td>
        </tr>
      ))}
    </tbody>
  </table>
);

function App() {


  const [employees, setEmployees] = useState<Employee[]>([]);
  const [selectedYear, setSelectedYear] = useState<number>(
    new Date().getFullYear()
  );

  useEffect(() => {
    populateEmployeeData();
  }, []);

  const handleYearChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedYear(parseInt(event.target.value));
  };

  const filteredContents = employees.map((employee) => (
    <tr key={employee.id}>
      <td>{employee.id}</td>
      <td>{employee.do_no}</td>
      <td>{employee.do_name}</td>
      <td>
        {employee.shifts
          .filter((shift) => shift.year === selectedYear)
          .map((shift, index) => (
            <div key={`${employee.id}-${shift.isOweek}-${index}`}>
              <strong>Week:</strong> {shift.isOweek}, <strong>Year:</strong>{" "}
              {shift.year} <br />
              Mon-{shift.monday.workdays_id}, Tue-{shift.tuesday.workdays_id},
              Wed-{shift.wednesday.workdays_id}, Thu-
              {shift.thursday.workdays_id}, Fri-{shift.friday.workdays_id}, Sat-
              {shift.saturday.workdays_id}, Sun-{shift.sunday.workdays_id}
            </div>
          ))}
      </td>
    </tr>
  ));

  async function populateEmployeeData() {
    try {
      const response = await fetch("https://mongoaspapi.azurewebsites.net/api/employee/");
      console.log(response.status);
      if (!response.ok) {
        console.error(`Error fetching: ${response.status}`);
        return;
      }
      const data = await response.json();
      setEmployees(data);
    } catch (error) {
      console.error("Failed to fetch employees:", error);
    }
  }

  return (
    <div>
      <h1 id="tableLabel">Employee Shifts</h1>
      <p>This component demonstrates fetching data from the server.</p>

      <div>
        <label htmlFor="yearSelect">Filter by Year: </label>
        <select
          id="yearSelect"
          value={selectedYear}
          onChange={handleYearChange}
        >
          <option value={2023}>2023</option>
          <option value={2024}>2024</option>
          {/* Add more years as needed */}
        </select>
      </div>

      <table className="table table-striped" aria-labelledby="tableLabel">
        <thead>
          <tr>
            <th>ID</th>
            <th>DO Number</th>
            <th>Name</th>
            <th>Shifts</th>
          </tr>
        </thead>
        <tbody>{filteredContents}</tbody>
      </table>

      <div>
        <h2>Shift Types</h2>
        {shiftContent}
      </div>
    </div>
  );
}

export default App;
