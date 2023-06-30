import { SelectItem } from "./SelectInterface";

const createMapYears = (): SelectItem[] => {
  const currentYear = new Date().getFullYear();
  let years: SelectItem[] = [];

  for (let year = 2020; year <= currentYear; year++) {
    years.push({ value: year.toString(), viewValue: year.toString() });
  }

  return years;
}

export const SelectYear = createMapYears();
