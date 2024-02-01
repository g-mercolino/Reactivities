import { makeAutoObservable, runInAction } from "mobx";
import { Activity } from "../models/activity";
import agent from "../api/agente";
import {v4 as uuid} from "uuid";

export default class ActivityStores {
    // title = 'Hello from Mobx';
    // activities: Activity[] = [];

    //the first parameter (string) is the Key 
    activityRegistry = new Map<string, Activity>();
    selectedActivity: Activity | undefined = undefined;
    editMode = false;
    loading = false;
    loadingInitial = false;


    constructor() {
        // makeObservable(this, {
        //     title: observable,
        //     setTitle: action.bound --> con bound associa automaticamente la funzione setTitle alla classe (ActivityStores)
        //     setTitle: action
        //}) 
        makeAutoObservable(this)
    }

    get activitiesByDate() {
        return Array.from(this.activityRegistry.values()).sort((a,b) => Date.parse(a.date) - Date.parse(b.date));
    }

    get groupedActivities() {
        return Object.entries(
            this.activitiesByDate.reduce((activities, activity) => {
                const date = activity.date;
                activities[date] = activities[date] ? [...activities[date], activity] : [activity];
                return activities;
            }, {} as {[key: string]: Activity[]})
        )
    }

    // setTitle() {
    //     this.title = this.title + '!';
    // }

    //se non vogliamo utilizzare bound  possiamo
    //associare alla classe con l'arrow function

    // setTitle = () => {
    //     this.title = this.title + '!';
    // }

    loadActivities = async () => {    
        this.setLoadingInitial(true);
        try {
            // this.activities = await agent.Acitivities.list();
            //     // updating state
            //     this.activities.forEach(activity => {
            //         activity.date = activity.date.split('T')[0];
            //         this.activities.push(activity);
            //     })

            const activities = await agent.Acitivities.list();
            // updating state
                activities.forEach(activity => {
                    this.setActivity(activity);
            })
            this.setLoadingInitial(false);
            
            
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    loadActivity = async (id: string) => {
        let activity = this.getActivity(id);
        if (activity) {
            this.selectedActivity = activity;
            return activity;
        } else {
            this.setLoadingInitial(true);
            try {
                activity = await agent.Acitivities.details(id);
                this.setActivity(activity);
                runInAction(() => this.selectedActivity = activity);
                this.setLoadingInitial(false);
                return activity;
            } catch (error) {
                console.log(error);
                this.setLoadingInitial(false);
            }
        }
    }

    private setActivity = (activity: Activity) => {
        activity.date = activity.date.split('T')[0];
        this.activityRegistry.set(activity.id, activity);
    }

    private getActivity = (id: string) => {
        return this.activityRegistry.get(id);
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }

    // selectActivity = (id: string) => {
    //     this.selectedActivity = this.activities.find(a => a.id === id);
    // }

    createActivity = async (activity: Activity) => {
        this.loading = true;
        activity.id = uuid();
        try {
            await agent.Acitivities.create(activity);
            runInAction(() => {
                this.activityRegistry.set(activity.id, activity);
                this.selectedActivity = activity;
                this.editMode = false;
                this.loading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
            })
        }
    }

    updatedActivity = async (activity: Activity) => {
        this.loading = true;
        try {
            await agent.Acitivities.update(activity);
            runInAction(() => {
            //   this.activities = [...this.activities.filter(a => a.id !== activity.id), activity];
              this.activityRegistry.set(activity.id, activity);
              this.selectedActivity = activity;
              this.editMode = false;
              this.loading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
            })
        }
    }

    deleteActivity = async (id: string) => {
        this.loading = true;
        try {
            await agent.Acitivities.delete(id);
            runInAction(() => {
                // this.activities = [...this.activities.filter(a => a.id !== id)];
                this.activityRegistry.delete(id);
                this.loading = false;
              })
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
            })
        }
    }



}