export class ListCategory{
  message? : string;
  result? : Data;
  state? : boolean;
}
export class Data{
  data? : Array<Category>;
}
export class Category{
  id? : string;
  name?: string;
}
