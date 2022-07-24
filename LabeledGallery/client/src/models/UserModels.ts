type ObjectsDetectionProvider = "Gcp";

export interface Account {
  name: string;
  email: string;
}

export interface UserInfoDto {
  isAuthenticated: boolean;
  account: Account;
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  name: string;
  email: string;
  password: string;
  objectsDetectionProvider: ObjectsDetectionProvider;
}
