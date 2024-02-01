import React, { useEffect } from "react";
import { Button, Card, Grid, Image } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { observer } from "mobx-react-lite";
import { Link, useParams } from "react-router-dom";
import ActivityDetailedHeader from "./ActivityDelaitedHeader";
import ActivityDetailedInfo from "./ActivityDetailedInfo";
import ActivityDetailedChat from "./ActivityDetailedChat";
import ActivityDetailedSideBar from "./ActivityDetailedSidebar";

//con fluid occuperà tutto lo spazio disponibile all'interno della griglia
export default observer(function ActivityDetails() {
    const {activityStore} = useStore();
    const {selectedActivity: activity, loadActivity, loadingInitial} = activityStore;
    const {id} = useParams();

    useEffect(() => {
        if(id) loadActivity(id);
    }, [id, loadActivity])

    if(loadingInitial || !activity) return <LoadingComponent/>;

    return (
       <Grid>
        <Grid.Column width={10}>
            <ActivityDetailedHeader activity={activity}/>
            <ActivityDetailedInfo activity={activity}/>
            <ActivityDetailedChat/>
        </Grid.Column>
        <Grid.Column width={6}>
            <ActivityDetailedSideBar/>
        </Grid.Column>
       </Grid>
    )
})