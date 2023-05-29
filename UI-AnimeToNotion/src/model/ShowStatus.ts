enum ShowStatusEnum {
  ToWatch,
  Watching,
  Completed,
  Dropped
}

export const ShowStatus = new Map<number, string>([
  [ShowStatusEnum.ToWatch, 'To Watch'],
  [ShowStatusEnum.Watching, 'Watching'],
  [ShowStatusEnum.Completed, 'Completed'],
  [ShowStatusEnum.Dropped, 'Dropped']
]);
