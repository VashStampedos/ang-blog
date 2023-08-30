import { Article } from "./article";
import { User } from "./user";


export class Blog{
    
    
    // constructor( id:number, name:string, idUser:number, user:User,articles?:Article[]) {
    //     this.id=id;
    //     this.name=name;
    //     this.idUser=idUser;
    //     this.user=user;
    //     this.articles = articles;
        
    // }
    id?:number;
    name?:string;
    idUser?:number;
    user?: User;
    articles?: Article[];
}