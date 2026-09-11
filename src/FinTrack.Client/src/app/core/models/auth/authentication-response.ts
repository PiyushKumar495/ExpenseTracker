export interface AuthenticationResponse
{
    accessToken:string;
    refreshToken:string;
    user:UserSummaryResponse;
}

export interface UserSummaryResponse
{
    id:string;
    firstName:string;
    lastName:string;
    email:string;
    currency:string;
    timeZone:string;
}