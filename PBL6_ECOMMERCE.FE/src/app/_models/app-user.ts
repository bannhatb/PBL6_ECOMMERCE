export class User {
  username?: string;
  password?: string;
}
export class UserToken {
  username?: string = "";
  token?: string = "";
}

export class RegisterUser {
  username? : string;
  password? : string;
  verifyPassword? : string;
  email? : string;
  gender? : boolean;
  role? : number
}
