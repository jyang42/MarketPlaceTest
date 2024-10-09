import { JobDTO, BidDTO } from '../models/types';
import axios from 'axios'

export const uri = 'http://localhost:32774/api/'

export const user_login = async(username: any, password: any) => {
    try{
        return await axios.post(uri + 'auth/login', {
            username,
            password,
        });
    }
    catch(error){
        throw new Error('Unauthorized')
    }

}

export const verifyToken = async () => {
    try{
        const token = localStorage.getItem('jwtToken');
        await axios.get(uri + 'auth/verify',{
            headers: {
                'Authorization': token 
            }
        });}
        catch(error) {
            throw new Error('Unauthorized')
        }
}

export const fetchActiveJobs = async (): Promise<JobDTO[]> => {
    const token = localStorage.getItem('jwtToken');
    const header = {
        'Authorization': `${token}`
    };
    const response = await fetch(uri + 'job/ActiveJobs',{headers:header});
    if (!response.ok) {
        throw new Error('Failed to fetch jobs');
    }
    const data = await response.json();
    return Array.isArray(data) ? data : []; 
};


export const fetchRecentJobs = async (): Promise<JobDTO[]> => {
    const token = localStorage.getItem('jwtToken');
    const header = {
        'Authorization': `${token}`
    };
    const response = await fetch(uri + 'job/RecentJobs',{headers: header});
    if (!response.ok) {
        throw new Error('Failed to fetch jobs');
    }
    const data = await response.json();
    return Array.isArray(data) ? data : []; 
};


export const fetchJob = async (id:any): Promise<JobDTO> => {
    const token = localStorage.getItem('jwtToken');
    const header = {
        'Authorization': `${token}`
    };
    const response = await fetch(uri + `job/${id}`,{headers:header})
    if (!response.ok) {
        throw new Error('Failed to fetch jobs',)
    }
    const data = await response.json();
    return data ? data: {} 
}


export const CreateJob = async (job:any): Promise<JobDTO> => {
    const  token = localStorage.getItem('jwtToken');
    const header = {
        'Authorization': `${token}`,
        'Content-Type': 'application/json',
    };
    const response = await fetch(uri + 'job/CreateJob', {
        method: 'POST',
        headers: header,
        body: JSON.stringify(job),
    });

    if (!response.ok) {
        throw new Error('Failed to create job');
    }
    const data = await response.json();
    return data
}


export const CreateBid = async (bid:any): Promise<BidDTO> => {
    const  token = localStorage.getItem('jwtToken');
    const header = {
        'Authorization': `${token}`,
        'Content-Type': 'application/json',
    };
    const response = await fetch(uri + 'job/CreateBid', {
        method: 'POST',
        headers: header,
        body: JSON.stringify(bid),
    });

    if (!response.ok) {
        throw new Error('Failed to create bid');
    }
    const data = await response.json();
    return data
}


