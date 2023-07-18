import { SelectItem } from "./SelectInterface";

const createMapYears = (): SelectItem[] => {
  const toYear = new Date().getFullYear()+1;
  let years: SelectItem[] = [];

  for (let year = 2020; year <= toYear; year++) {
    years.push({ value: year.toString(), viewValue: year.toString() });
  }

  return years;
}

export const SelectYear = createMapYears();
