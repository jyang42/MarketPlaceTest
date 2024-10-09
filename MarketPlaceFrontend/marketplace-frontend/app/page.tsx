'use client';
import useAuth from '@/hooks/useAuth';
import React, { useEffect, useState } from 'react';
import { fetchActiveJobs, fetchRecentJobs } from '../utils/api';
import JobGrid from '../components/jobgrid';
import { JobDTO } from '../models/types';

const HomePage: React.FC = () => {
    const [activeJobs, setActiveJobs] = useState<JobDTO[]>([]);
    const [recentJobs, setRecentJobs] = useState<JobDTO[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    useAuth()
    useEffect(() => {
        const loadActiveJobs = async () => {
            try {
                const data = await fetchActiveJobs();
                setActiveJobs(data);
            } catch (error) {
                console.log(error)
                setError('Failed to load jobs');
            } finally {
                setLoading(false);
            }
        };

        loadActiveJobs();
    }, []);
    useEffect(() => {
      const loadRecentJobs = async () => {
          try {
              const data = await fetchRecentJobs();
              setRecentJobs(data);
          } catch (error) {
              console.log(error)
              setError('Failed to load jobs');
          } finally {
              setLoading(false);
          }
      };

      loadRecentJobs();
  }, []);

    if (loading) return <p className="text-center">Loading...</p>;
    if (error) return <p className="text-center text-red-500">{error}</p>;

    return (
        <div>
            <h1 className="text-center text-3xl font-bold my-6">Top 5 Active Jobs</h1>
            <JobGrid jobs={activeJobs} />
            <h1 className="text-center text-3xl font-bold my-6">Top 5 Recent Jobs</h1>
            <JobGrid jobs={recentJobs} />
        </div>
        
    );
};

export default HomePage;