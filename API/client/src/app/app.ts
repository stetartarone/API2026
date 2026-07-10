import { Component,inject,OnInit,signal} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit
{
    private http = inject(HttpClient);
    protected readonly title ='App Elenco Membri';
    protected members = signal<any>([]);
    async ngOnInit(): Promise<void> 
    {
      this.members.set(await this.getMembers());
    }
    async getMembers()
    {
      try
      {
        return lastValueFrom(this.http.get('http://localhost:5159/api/members'));
      }
      catch(err)
      {
        console.log(err);
        throw err;
      }
    }
}
